using Checkers.Api.Handlers.Common;
using Checkers.Api.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Api.Domain
{
    public class Board
    {
        const int DEFAULT_SIZE = 10;
        const int DEFAULT_PAWN_ROWS = 3;
        const bool DEFAULT_LONG_MOVES = false;

        public int Size { get; private set; }

        public Field[] Fields { get; private set; }
        // below not functional yet
        private bool isLongMoveAllowed;

        public PawnColor CurrentTurn { get; private set; }
        private int? _multipleMovesIndex = null;

        private int WhiteForwardLeft => Size + 1;
        private int WhiteForwardRight => Size - 1;
        private int BlackForwardLeft => -Size + 1;
        private int BlackForwardRight => -Size - 1;

        public static Board CreateNew(int size = DEFAULT_SIZE, int pawnRows = DEFAULT_PAWN_ROWS, bool longMovesAllowed = DEFAULT_LONG_MOVES)
        {
            var board = new Board
            {
                Size = size,
                isLongMoveAllowed = longMovesAllowed,
                CurrentTurn = PawnColor.White
            };
            board.InitializeFieldsAndPawns(pawnRows);
            return board;
        }

        public static Board FromStanpshot(BoardDbModel dbModel)
        {
            var board = new Board
            {
                Size = dbModel.Size,
                isLongMoveAllowed = dbModel.LongMoveAllowed,
                CurrentTurn = PawnColor.FromName(dbModel.CurrentTurn)
            };
            board.InitializeEmptyFields();
            foreach (PawnDbModel pawn in dbModel.Pawns)
            {
                var color = PawnColor.FromName(pawn.Color);
                board.Fields[pawn.Index].PutPawn(new Pawn(color, pawn.IsKing));
            }
            return board;
        }

        public BoardDbModel ToSnapshot()
        {
            return new BoardDbModel
            {
                Size = Size,
                LongMoveAllowed = isLongMoveAllowed,
                CurrentTurn = CurrentTurn.Name,
                Pawns = Fields
                .Where(f => !Field.IsEmpty(f))
                .Select(f => new PawnDbModel { Color = f?.Pawn?.Color?.Name, Index = f.Index, IsKing = f?.Pawn?.IsKing == true })
                .ToList()
            };
        }

        public IEnumerable<Move> GetAvailableMoves()
        {
            if (!_multipleMovesIndex.HasValue)
            {
                return GetAvailableMovesForColor(CurrentTurn);
            }
            var directions = GetPawnDirections(_multipleMovesIndex.Value);
            return GetJumpsForIndex(_multipleMovesIndex.Value, directions);
        }

        public int GetNumberOfPawns(PawnColor color)
        {
            return Fields.Where(f => !Field.IsEmpty(f) && f.Pawn.Color.Equals(color)).Count();
        }


        private IEnumerable<Move> GetAvailableMovesForColor(PawnColor color)
        {
            List<Move> result = new List<Move>();
            bool hasAnyJumps = false;
            foreach (var field in Fields)
            {
                if (field.Pawn?.Color != color)
                {
                    continue;
                }
                var directions = GetPawnDirections(field.Index);
                var jumps = GetJumpsForIndex(field.Index, directions);
                result.AddRange(jumps);
                if (jumps.Any())
                {
                    hasAnyJumps = true;
                    result.RemoveAll(x => !x.IsJump);
                }
                if (!hasAnyJumps)
                {
                    var simpleMoves = GetSimpleMovesForIndex(field.Index, directions);
                    result.AddRange(simpleMoves);
                }
            }
            return result;
        }

        private IEnumerable<Move> GetJumpsForIndex(int index, IEnumerable<int> directions)
        {
            var jumps = directions
                // target is inside the board, in two next rows, next field is occupied by an enemy and target is empty
                .Where(x => index + 2 * x > 0 && index + 2 * x < Fields.Length && Math.Abs(index / Size - (index + 2 * x) / Size) == 2 && !Fields[index + x].Empty && Fields[index + x].Pawn.Color != Fields[index].Pawn.Color && Fields[index + x + x].Empty)
                .Select(x => new Move(index, index + 2 * x, index + x));
            return jumps;
        }

        private IEnumerable<Move> GetSimpleMovesForIndex(int index, IEnumerable<int> directions)
        {
            var moves = directions
                .Where(x => index + x > 0 && index + x < Fields.Length && Math.Abs(index / Size - (index + x) / Size) == 1 && Fields[index + x].Empty)
                .Select(x => new Move(index, index + x));
            return moves;
        }

        private IEnumerable<int> GetPawnDirections(int index)
        {
            List<int> directions = new List<int>();
            if (Fields[index].Pawn.IsKing)
            {
                directions.AddRange(new int[] { WhiteForwardLeft, WhiteForwardRight, BlackForwardLeft, BlackForwardRight });
            }
            else
            {
                directions.AddRange(Fields[index].Pawn.Color == PawnColor.White ? new int[] { WhiteForwardLeft, WhiteForwardRight } : new int[] { BlackForwardLeft, BlackForwardRight });
            }
            return directions.Where(x => index + x > 0 && index + x < Fields.Length);
        }

        public void MovePawn(Move move)
        {
            ApplyMove(move.StartIndex, move.TargetIndex, move.JumpOverIndex);
            // if target index is at the end of the board, switch to king
            if(!Fields[move.TargetIndex].Pawn.IsKing && IsKingEdge(move.TargetIndex))
            {
                Fields[move.TargetIndex].Pawn.MakeKing();
            }
            // if the move was a jump and that particular pawn has any more possible jumps, keep the turn as it is 
            if (move.JumpOverIndex.HasValue && GetJumpsForIndex(move.TargetIndex, GetPawnDirections(move.TargetIndex)).Any())
            {
                _multipleMovesIndex = move.TargetIndex;
            }
            else
            {
                SwitchTurns();
            }
        }

        private bool IsKingEdge(int index)
        {
            int minEdgeIndex = Fields[index].Pawn.Color.Equals(PawnColor.White) ? Size * Size - Size : 0;
            return index >= minEdgeIndex && index <= minEdgeIndex + Size - 1;
        }

        private void ApplyMove(int startIndex, int targetIndex, int? jumpOverIndex = null)
        {
            if (startIndex < 0 || startIndex >= Fields.Length)
            {
                throw new ArgumentException("Start index out of range", nameof(startIndex));
            }
            if (targetIndex < 0 || targetIndex >= Fields.Length)
            {
                throw new ArgumentException("Target index out of range", nameof(targetIndex));
            }
            if (jumpOverIndex.HasValue && (jumpOverIndex.Value < 0 || jumpOverIndex.Value >= Fields.Length))
            {
                throw new ArgumentException("Jump over index out of range", nameof(jumpOverIndex));
            }
            if (Fields[startIndex].Empty || !Fields[targetIndex].Empty)
            {
                throw new InvalidMoveException("Start index needs to be occupied by a player and target index needs to be empty");
            }
            int distance = Math.Abs(targetIndex - startIndex);
            int minDistance = jumpOverIndex.HasValue ? (Size - 1) * 2 : Size - 1;
            int maxDistance = jumpOverIndex.HasValue ? (Size + 1) * 2 : Size + 1;
            if (!isLongMoveAllowed && (distance < minDistance || distance > maxDistance))
            {
                throw new InvalidMoveException("Long moves are not allowed");
            }
            if (!Fields[targetIndex].IsUsable)
            {
                throw new InvalidMoveException("Target field is not usable");
            }
            if (jumpOverIndex.HasValue && Fields[startIndex].Pawn == Fields[jumpOverIndex.Value].Pawn)
            {
                throw new InvalidMoveException("Jumping over the same player is not allowed");
            }
            if (!IsProperPlayerMoving(startIndex))
            {
                throw new InvalidGameOperation($"Cannot move pawn from index {startIndex}. Current turn is {CurrentTurn.Name}.");
            }
            Fields[targetIndex].PutPawn(Fields[startIndex].Pawn);
            Fields[startIndex].Clear();
            if (jumpOverIndex.HasValue)
            {
                Fields[jumpOverIndex.Value].Clear();
            }
        }

        private void SwitchTurns()
        {
            CurrentTurn = CurrentTurn.Equals(PawnColor.White) ? PawnColor.Black : PawnColor.White;
            _multipleMovesIndex = null;
        }

        private bool IsProperPlayerMoving(int startIndex)
        {
            return Fields[startIndex].Pawn.Color.Equals(CurrentTurn);
        }

        private void InitializeEmptyFields()
        {
            Fields = new Field[Size * Size];
            for (int i = 0; i < Fields.Length; i++)
            {
                int row = i / Size;
                int column = i % Size;
                bool isBlack = (row + column) % 2 == 0;
                Fields[i] = Field.Create(isBlack, i);
            }
        }

        private void InitializeFieldsAndPawns(int pawnRows)
        {
            Fields = new Field[Size * Size];
            for (int i = 0; i < Fields.Length; i++)
            {
                int row = i / Size;
                int column = i % Size;
                bool isBlack = (row + column) % 2 == 0;
                Fields[i] = Field.Create(isBlack, i);
                if (!isBlack)
                {
                    continue;
                }
                if (row < pawnRows)
                {
                    Fields[i].PutPawn(new Pawn(PawnColor.White));
                }
                if (row >= Size - pawnRows)
                {
                    Fields[i].PutPawn(new Pawn(PawnColor.Black));
                }
            }
        }
    }
}