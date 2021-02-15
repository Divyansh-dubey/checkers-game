using AutoMapper;
using Checkers.Api.Handlers.Common;
using Checkers.Api.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Checkers.Api.Handlers
{
    public class AvailableGamesQuery : IRequest<AvailableGamesResult>
    {
    }

    public class AvailableGamesHandler : IRequestHandler<AvailableGamesQuery, AvailableGamesResult>
    {
        private readonly GamesRepository _repo;
        private readonly IMapper _mapper;

        public AvailableGamesHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        public async Task<AvailableGamesResult> Handle(AvailableGamesQuery request, CancellationToken cancellationToken)
        {
            var allGames = await _repo.GetAvailableGamesAsync();
            return new AvailableGamesResult
            {
                AvailableGames = _mapper.Map<List<GameData>>(allGames)
            };
        }
    }

    public class AvailableGamesResult
    {
        public IEnumerable<GameData> AvailableGames { get; set; }
    }
}
