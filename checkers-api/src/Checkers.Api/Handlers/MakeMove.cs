using AutoMapper;
using Checkers.Api.Domain;
using Checkers.Api.Handlers.Common;
using Checkers.Api.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Checkers.Api.Handlers
{
    public class MakeMoveRequest : IRequest<MakeMoveResponse>
    {
        public Guid GameId { get; set; }
        public MoveData Move { get; set; }
    }

    public class MakeMoveHandler : IRequestHandler<MakeMoveRequest, MakeMoveResponse>
    {
        private readonly GamesRepository _repo;
        private readonly IMapper _mapper;

        public MakeMoveHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public async Task<MakeMoveResponse> Handle(MakeMoveRequest request, CancellationToken cancellationToken)
        {
            var gameSnapshot = await _repo.GetByIdAsync(request.GameId);
            Game game = Game.FromSnapshot(gameSnapshot);
            game.MakeMove(_mapper.Map<Move>(request.Move));
            await _repo.Update(game.ToSnapshot());
            return new MakeMoveResponse
            {
                Board = new BoardData
                {
                    Size = game.BoardSize,
                    Fields = game.Board.Fields.Select(f => new FieldData
                    {
                        Index = f.Index,
                        IsUsable = f.IsUsable,
                        PawnColor = f.Pawn?.Color.Name,
                        IsKing = f.Pawn?.IsKing == true
                    }),
                    AvailableMoves = _mapper.Map<IEnumerable<MoveData>>(game.Board.GetAvailableMoves())
                },
                CurrentTurn = game.Board.CurrentTurn.Name,
                Winner = game.IsOn ? null : new WinnerData { Id = game.Winner.Id }
            };
        }
    }

    public class MakeMoveResponse
    {
        public BoardData Board { get; set; }
        public string CurrentTurn { get; set; }
        public WinnerData Winner { get; set; }
    }
}
