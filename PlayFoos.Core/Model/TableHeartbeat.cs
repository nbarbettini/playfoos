using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class TableHeartbeat
    {
        [BsonElement("lastAt")]
        public DateTime LastUpdatedAt { get; set; }

        [BsonElement("lastErr")]
        public string LastError { get; set; }
    }
}
