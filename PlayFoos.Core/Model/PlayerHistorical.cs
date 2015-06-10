using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class PlayerHistorical : IEquatable<PlayerHistorical>, IDeepClonable<PlayerHistorical>
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("rOld")]
        public int StartRating { get; set; }

        [BsonElement("rNew")]
        public int EndRating { get; set; }

        // Default constructor
        public PlayerHistorical()
        {
            Id = Guid.NewGuid();
        }

        public PlayerHistorical(PlayerActive source)
        {
            Id = source.Id;
            Name = source.Name;
            StartRating = source.Rating;
        }

        #region IEquatable

        public bool Equals(PlayerHistorical other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlayerHistorical;
            if (obj == null)
                return false;

            return new PlayerHistoricalEqualityComparer().Equals(this, other);
        }

        public override int GetHashCode()
        {
            return new PlayerHistoricalEqualityComparer().GetHashCode(this);
        }

        public static bool operator ==(PlayerHistorical x, PlayerHistorical y)
        {
            return new PlayerHistoricalEqualityComparer().Equals(x, y);
        }

        public static bool operator !=(PlayerHistorical x, PlayerHistorical y)
        {
            return !(x == y);
        }

        #endregion

        #region IDeepClonable

        public PlayerHistorical DeepClone()
        {
            return new PlayerHistorical()
            {
                Id = Id,
                Name = Name,
                StartRating = StartRating,
                EndRating = EndRating
            };
        }

        #endregion
    }

    #region IEqualityComparer

    public class PlayerHistoricalEqualityComparer : IEqualityComparer<PlayerHistorical>
    {
        public bool Equals(PlayerHistorical x, PlayerHistorical y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            // Compare by value equality
            var result =
                x.Id == y.Id &&
                x.Name.Equals(y.Name) &&
                x.StartRating == y.StartRating &&
                x.EndRating == x.EndRating;
            return result;
        }

        public int GetHashCode(PlayerHistorical obj)
        {
            // Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            return HashCode.Start
                .Hash(obj.Id)
                .Hash(obj.Name)
                .Hash(obj.StartRating)
                .Hash(obj.EndRating);
        }
    }

    #endregion
}
