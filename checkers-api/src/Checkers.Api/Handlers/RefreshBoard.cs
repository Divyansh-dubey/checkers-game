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
    public class RefreshBoardRequest: IRequest<RefreshBoardResult>
    {
        public Guid GameId { get; set; }
    }

    public class RefreshBoardHandler : IRequestHandler<RefreshBoardRequest, RefreshBoardResult>
    {
        private readonly GamesRepository _repo;
        private readonly IMapper _mapper;

        public RefreshBoardHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public async Task<RefreshBoardResult> Handle(RefreshBoardRequest request, CancellationToken cancellationToken)
        {
            var gameSnapshot = await _repo.GetByIdAsync(request.GameId);
            Game game = Game.FromSnapshot(gameSnapshot);
            return new RefreshBoardResult
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
                CurrentTurn = game.Board.CurrentTurn.Name
            };
        }
    }

    public class RefreshBoardResult
    {
        public BoardData Board { get; set; }
        public string CurrentTurn { get; set; }
    }
}
