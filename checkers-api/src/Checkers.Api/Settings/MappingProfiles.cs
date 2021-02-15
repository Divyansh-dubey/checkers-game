using AutoMapper;
using Checkers.Api.Domain;
using Checkers.Api.Handlers.Common;
using Checkers.Api.Repositories.Models;

namespace Checkers.Api.Settings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<GameDbModel, GameData>();
            CreateMap<GameData, GameDbModel>();

            CreateMap<Game, GameDbModel>();

            CreateMap<Move, MoveData>();
            CreateMap<MoveData, Move>();
        }
    }
}
