using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;

        public MongoContext(string connectionString, string database)
        {
            _client = new MongoClient(connectionString);
            _db = _client.GetDatabase(database);
        }

        public IMongoCollection<T> GetCollection<T>(string collection)
        {
            return _db.GetCollection<T>(collection);
        }
    }
}
