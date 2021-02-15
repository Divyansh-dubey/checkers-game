using AutoMapper;
using Checkers.Api.Domain;
using Checkers.Api.Repositories;
using Checkers.Api.Repositories.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Checkers.Api.Handlers
{
    public class CreateGameRequest : IRequest<CreateGameResult>
    {
        public string GameName { get; set; }
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
    }

    public class CreateGameHandler : IRequestHandler<CreateGameRequest, CreateGameResult>
    {
        private readonly GamesRepository _repo;
        private readonly IMapper _mapper;
        public CreateGameHandler(GamesRepository repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }
        public async Task<CreateGameResult> Handle(CreateGameRequest request, CancellationToken cancellationToken)
        {
            Game newGame = Game.CreateNew(request.GameName);
            await _repo.Add(newGame.ToSnapshot());
            return new CreateGameResult
            {
                CreatorId = request.CreatorId,
                CreatorName = request.CreatorName,
                Name = newGame.Name,
                Id = newGame.Id
            };
        }
    }

    public class CreateGameResult
    {
        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
    }
}
