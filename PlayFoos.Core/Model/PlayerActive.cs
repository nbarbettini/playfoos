using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class PlayerActive : IEquatable<PlayerActive>, IDeepClonable<PlayerActive>
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("r")]
        public int Rating { get; set; }

        // Default constructor
        public PlayerActive()
        {
            Id = Guid.NewGuid();
            Rating = 1500;
        }

        #region IEquatable

        public bool Equals(PlayerActive other)
        {
            return this.Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as PlayerActive;
            if (obj == null)
                return false;

            return new PlayerActiveEqualityComparer().Equals(this, other);
        }

        public override int GetHashCode()
        {
            return new PlayerActiveEqualityComparer().GetHashCode(this);
        }

        public static bool operator ==(PlayerActive x, PlayerActive y)
        {
            return new PlayerActiveEqualityComparer().Equals(x, y);
        }

        public static bool operator !=(PlayerActive x, PlayerActive y)
        {
            return !(x == y);
        }

        #endregion

        #region IDeepClonable

        public PlayerActive DeepClone()
        {
            return new PlayerActive()
            {
                Id = this.Id,
                Name = this.Name,
                Rating = this.Rating
            };
        }

        #endregion

    }

    #region IEqualityComparer

    public class PlayerActiveEqualityComparer : IEqualityComparer<PlayerActive>
    {
        public bool Equals(PlayerActive x, PlayerActive y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            // Compare by value equality
            var result =
                x.Id == y.Id &&
                x.Name.Equals(y.Name) &&
                x.Rating == y.Rating;
            return result;
        }

        public int GetHashCode(PlayerActive obj)
        {
            // Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            return HashCode.Start
                .Hash(obj.Id)
                .Hash(obj.Name)
                .Hash(obj.Rating);
        }
    }

    #endregion
}
