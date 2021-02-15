using AutoMapper;
using Checkers.Api.Handlers.Common;
using Checkers.Api.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Checkers.Api.Handlers
{
    public class JoinLobbyRequest : IRequest<JoinLobbyResult>
    {
        public Guid GameId { get; set; }
        public Guid PlayerId { get; set; }
        public string PlayerName { get; set; }
    }

    public class JoinLobbyHandler : IRequestHandler<JoinLobbyRequest, JoinLobbyResult>
    {
        private readonly GamesRepository _repo;
        private readonly IMapper _mapper;
        public JoinLobbyHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }
        public async Task<JoinLobbyResult> Handle(JoinLobbyRequest request, CancellationToken cancellationToken)
        {
            var allGames = await _repo.GetAvailableGamesAsync();
            return new JoinLobbyResult
            {
                Games = _mapper.Map<List<GameData>>(allGames)
            };
        }
    }

    public class JoinLobbyResult
    {
        public IEnumerable<GameData> Games { get; set; }
    }
}
