using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class Player : IEquatable<Player>, IDeepClonable<Player>
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime Created { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("e")]
        public string Email { get; set; }

        [BsonElement("pw")]
        public string Password { get; set; }

        [BsonElement("win")]
        public int CareerWins { get; set; }

        [BsonElement("loss")]
        public int CareerLosses { get; set; }

        [BsonElement("r")]
        public int Rating { get; set; }

        // Default constructor
        public Player()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;

            Rating = 1500;
        }

        #region IEquatable

        public bool Equals(Player other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Player;
            if (obj == null)
                return false;

            return new PlayerEqualityComparer().Equals(this, other);
        }

        public override int GetHashCode()
        {
            return new PlayerEqualityComparer().GetHashCode(this);
        }

        public static bool operator ==(Player x, Player y)
        {
            return new PlayerEqualityComparer().Equals(x, y);
        }

        public static bool operator !=(Player x, Player y)
        {
            return !(x == y);
        }

        #endregion

        #region IDeepClonable

        public Player DeepClone()
        {
            return new Player()
            {
                Id = Id,
                Created = Created,
                Name = Name,
                Email = Email,
                Password = Password,
                CareerWins = CareerWins,
                CareerLosses = CareerLosses,
                Rating = Rating
            };
        }

        #endregion
    }

    #region IEqualityComparer

    public class PlayerEqualityComparer : IEqualityComparer<Player>
    {
        public bool Equals(Player x, Player y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            // Compare by value equality
            var result =
                x.Id == y.Id &&
                x.Created == y.Created &&
                x.Name == y.Name &&
                x.Email == y.Email &&
                x.Password == y.Password &&
                x.CareerWins == y.CareerWins &&
                x.CareerLosses == y.CareerLosses &&
                x.Rating == y.Rating;
            return result;
        }

        public int GetHashCode(Player obj)
        {
            // Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            return HashCode.Start
                .Hash(obj.Id)
                .Hash(obj.Created)
                .Hash(obj.Name)
                .Hash(obj.Email)
                .Hash(obj.Password)
                .Hash(obj.CareerWins)
                .Hash(obj.CareerLosses)
                .Hash(obj.Rating);
        }
    }

    #endregion
}
