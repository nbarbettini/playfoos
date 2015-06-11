using MongoDB.Bson;
using MongoDB.Driver;
using PlayFoos.Core.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public class TableHeartbeatService : ITableHeartbeatService
    {
        private readonly IMongoCollection<Model.TableHeartbeat> _collection;

        public TableHeartbeatService(IMongoContext context)
        {
            _collection = context.GetCollection<Model.TableHeartbeat>("TableHeartbeat");
        }

        public async Task<DateTime?> GetLatestAsync()
        {
            var result = await _collection.Find(new BsonDocument())
                .SortByDescending(x => x.LastUpdatedAt)
                .FirstOrDefaultAsync();

            if (result == null)
                return null;
            return result.LastUpdatedAt;
        }

        public async Task UpdateAsync(DateTime timestamp, string error)
        {
            await _collection.InsertOneAsync(new Model.TableHeartbeat()
            {
                LastUpdatedAt = timestamp,
                LastError = error
            });
        }
    }
}
