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
    public class JoinGameRequest : IRequest<JoinGameResult>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
    }

    public class JoinGameHandler : IRequestHandler<JoinGameRequest, JoinGameResult>
    {
        private readonly GamesRepository _repo;
        public JoinGameHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
        }

        public async Task<JoinGameResult> Handle(JoinGameRequest request, CancellationToken cancellationToken)
        {
            var gameSnapshot = await _repo.GetByIdAsync(request.GameId);
            Game game = Game.FromSnapshot(gameSnapshot);
            game.AddPlayer(request.PlayerId, request.PlayerName);
            await _repo.Update(game.ToSnapshot());
            return new JoinGameResult
            {
                GameId = game.Id,
                GameName = game.Name,
                Players = game.Players.Select(x => new PlayerData { Id = x.Id, Name = x.Name, Color = x.Color.Name }),
                IsGameOn = game.IsOn
            };
        }
    }

    public class JoinGameResult
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; }
        public IEnumerable<PlayerData> Players { get; set; }
        public bool IsGameOn { get; set; }
    }
}
