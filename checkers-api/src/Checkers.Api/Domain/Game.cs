using Checkers.Api.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers.Api.Domain
{
    public class Game
    {
        private bool _hasStarted;
        private bool _hasEnded;
        public Player Winner { get; private set; }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        private IDictionary<Guid, Player> _players;
        public IEnumerable<Player> Players => _players.Values;
        public Player BlackPlayer => _players.Values.FirstOrDefault(x => x.Color.Equals(PawnColor.Black));
        public Player WhitePlayer => _players.Values.FirstOrDefault(x => x.Color.Equals(PawnColor.White));
        public bool HasAnyPlayers => BlackPlayer != null || WhitePlayer != null;
        public bool CanBeJoined => !_hasStarted && !_hasEnded && (BlackPlayer == null || WhitePlayer == null);
        public bool IsOn => _hasStarted && !_hasEnded && Winner is null;
        public Board Board { get; private set; }
        public int BoardSize => Board.Size;

        private Game()
        {
            _players = new Dictionary<Guid, Player>();
        }

        public static Game CreateNew(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty", nameof(name));
            }
            return new Game
            {
                Id = Guid.NewGuid(),
                Name = name,
                _hasStarted = false,
                _hasEnded = false,
                Winner = null,
                Board = Board.CreateNew()
            };
        }

        public static Game FromSnapshot(GameDbModel snapshot)
        {
            var game = new Game
            {
                Id = snapshot.Id,
                Name = snapshot.Name,
                _hasStarted = snapshot.HasStarted,
                _hasEnded = false,
                Winner = null,
                Board = Board.FromStanpshot(snapshot.Board)
            };
            if (snapshot.BlackPlayerId.HasValue)
            {
                game._players.Add(snapshot.BlackPlayerId.Value, Player.Create(snapshot.BlackPlayerId.Value, snapshot.BlackPlayerName, PawnColor.Black));
            }
            if (snapshot.WhitePlayerId.HasValue)
            {
                game._players.Add(snapshot.WhitePlayerId.Value, Player.Create(snapshot.WhitePlayerId.Value, snapshot.WhitePlayerName, PawnColor.White));
            }
            return game;
        }

        public GameDbModel ToSnapshot()
        {
            return new GameDbModel
            {
                Id = Id,
                Name = Name,
                BlackPlayerId = BlackPlayer?.Id,
                BlackPlayerName = BlackPlayer?.Name,
                WhitePlayerId = WhitePlayer?.Id,
                WhitePlayerName = WhitePlayer?.Name,
                HasStarted = _hasStarted,
                Board = Board.ToSnapshot()
            };
        }

        public void AddPlayer(Guid playerId, string playerName)
        {
            if (_hasStarted || _players.Count > 1)
            {
                throw new InvalidGameOperation($"Cannot join a game a running game");
            }

            PawnColor randomColor = GetRandomPlayerColor();
            if(_players.ContainsKey(playerId))
            {
                return;
            }
            Player newPlayer = Player.Create(playerId, playerName, randomColor);
            _players.Add(playerId, newPlayer);
            if (BlackPlayer != null && WhitePlayer != null)
            {
                _hasStarted = true;
            }
        }

        public void RemovePlayer(Guid playerId)
        {
            _players.Remove(playerId);
            if (_hasStarted && _players.Any())
            {
                Winner = _players.First().Value;
                _hasEnded = true;
            }
        }

        public void MakeMove(Move move)
        {
            Board.MovePawn(move);
            if(HasWhitePlayerWon())
            {
                Winner = WhitePlayer;
            }
            if(HasBlackPlayerWon())
            {
                Winner = BlackPlayer;
            }
            if(Winner != null)
            {
                _hasEnded = true;
            }
        }

        private PawnColor GetRandomPlayerColor()
        {
            if (_players.Count == 1)
            {
                var existingColor = _players.First().Value.Color;
                return new[] { PawnColor.White, PawnColor.Black }.First(x => !x.Equals(existingColor));
            }
            return new[] { PawnColor.White, PawnColor.Black }[new Random().Next(2)];
        }

        private bool HasWhitePlayerWon()
        {
            int blackPawns = Board.GetNumberOfPawns(PawnColor.Black);
            return blackPawns == 0;
        }

        private bool HasBlackPlayerWon()
        {
            int whitePawns = Board.GetNumberOfPawns(PawnColor.White);
            return whitePawns == 0;
        }
    }
}
