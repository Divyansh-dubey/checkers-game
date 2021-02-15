using Checkers.Api.Repositories.Models;
using Checkers.Api.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Checkers.Api.Repositories
{
    public class GamesRepository
    {
        private readonly IMongoCollection<GameDbModel> _games;

        public GamesRepository(ICheckersDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _games = database.GetCollection<GameDbModel>(settings.GamesCollectionName);
        }

        public async Task<List<GameDbModel>> GetAvailableGamesAsync()
        {
            var games = await _games.FindAsync(game => !game.BlackPlayerId.HasValue || !game.WhitePlayerId.HasValue);
            return games.ToList();
        }

        public async Task<GameDbModel> GetByIdAsync(Guid id)
        {
            var games = await _games.FindAsync(game => game.Id == id);
            return games.FirstOrDefault();
        }

        public async Task Add(GameDbModel game)
        {
            await _games.InsertOneAsync(game);
        }

        public async Task Update(GameDbModel game)
        {
            await _games.ReplaceOneAsync(x => x.Id == game.Id, game);
        }
    }
}
