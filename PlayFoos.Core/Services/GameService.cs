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
            {
                // Current behavior: create a new game if none exists
                // TODO: change this

                current = await NewAsync();
                await RemoveVolleyFlag(current);
            }

            if (_gameLogicService.IsGameOver(current))
                return false;

            // TODO move this into GameLogicService
            if (side == 0 && current.ScoreBlack + amount < 0)
                return false;
            if (side == 1 && current.ScoreYellow + amount < 0)
                return false;

            if (current.InVolley)
            {
                await RemoveVolleyFlag(current);
                return true;
            }

            UpdateDefinition<Model.Game> update;
            if (side == 0)
            {
                update = Builders<Model.Game>.Update.Set(x => x.ScoreBlack, current.ScoreBlack + amount);
            }
            else
            {
                update = Builders<Model.Game>.Update.Set(x => x.ScoreYellow, current.ScoreYellow + amount);
            }
            
            var result = await _collection.UpdateOneAsync(x => x.Id == current.Id, update);
            return result.IsAcknowledged;
        }

        private async Task<bool> RemoveVolleyFlag(Model.Game game)
        {
            UpdateDefinition<Model.Game> update;

            update = Builders<Model.Game>.Update.Set(x => x.InVolley, false);
            var result = await _collection.UpdateOneAsync(x => x.Id == game.Id, update);

            if (result.IsAcknowledged)
                game.InVolley = false;

            return result.IsAcknowledged;
        }

        public async Task<Model.Game> NewAsync(bool force = false)
        {
            if (!force)
            {
                var current = await GetCurrentAsync();
                if (current != null)
                    return null;
            }

            await CleanCurrentGames();

            var newGame = new Model.Game();
            await _collection.InsertOneAsync(newGame);
            return newGame;
        }

        private async Task CleanCurrentGames()
        {
            // Delete all
            await _collection.DeleteManyAsync(new BsonDocument());
        }
    }
}
