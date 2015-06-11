using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using PlayFoos.Core.Context;

namespace PlayFoos.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IMongoCollection<Model.Game> _collection;
        private readonly IGameLogicService _gameLogicService;

        public GameService(IMongoContext context, IGameLogicService gameLogicService)
        {
            _collection = context.GetCollection<Model.Game>("CurrentGame");
            _gameLogicService = gameLogicService;
        }

        public async Task<Model.Game> GetCurrentAsync()
        {
            return await _collection
                .Find(new BsonDocument())
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteCurrentAsync()
        {
            var current = await GetCurrentAsync();
            if (current == null)
                return true;

            var result = await _collection.DeleteOneAsync(x => x.Id == current.Id);
            return (result.IsAcknowledged && result.DeletedCount == 1);
        }

        public async Task<bool> UpdateScoreAsync(int side, int amount)
        {
            var current = await GetCurrentAsync();
            if (current == null)
                return false;

            if (_gameLogicService.IsGameOver(current))
                return false;

            UpdateDefinition<Model.Game> update;
            if (side == 0)
                update = Builders<Model.Game>.Update.Set(x => x.ScoreBlack, current.ScoreBlack + amount);
            else
                update = Builders<Model.Game>.Update.Set(x => x.ScoreYellow, current.ScoreYellow + amount);
            
            var result = await _collection.UpdateOneAsync(x => x.Id == current.Id, update);
            return result.IsAcknowledged;
        }

        public async Task<Model.Game> NewAsync()
        {
            var current = await GetCurrentAsync();
            if (current != null)
                return null;

            var newGame = new Model.Game();
            await _collection.InsertOneAsync(newGame);
            return newGame;
        }
    }
}
