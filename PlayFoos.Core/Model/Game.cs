using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class Game : IEquatable<Game>, IDeepClonable<Game>
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime Created { get; set; }

        [BsonElement("startedAt")]
        public DateTime? Started { get; set; }

        [BsonElement("inVolley")]
        public bool InVolley { get; set; }

        [BsonElement("bScore")]
        public int ScoreBlack { get; set; }

        [BsonElement("yScore")]
        public int ScoreYellow { get; set; }

        [BsonElement("bTeam")]
        public List<PlayerActive> TeamBlack { get; set; }

        [BsonElement("yTeam")]
        public List<PlayerActive> TeamYellow { get; set; }

        // Default constructor
        public Game()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            InVolley = true;

            TeamBlack = new List<PlayerActive>();
            TeamYellow = new List<PlayerActive>();
        }

        #region IEquatable

        public bool Equals(Game other)
        {
            return this.Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as Game;
            if (obj == null)
                return false;

            return new GameEqualityComparer().Equals(this, other);
        }

        public override int GetHashCode()
        {
            return new GameEqualityComparer().GetHashCode(this);
        }

        public static bool operator ==(Game x, Game y)
        {
            return new GameEqualityComparer().Equals(x, y);
        }

        public static bool operator !=(Game x, Game y)
        {
            return !(x == y);
        }

        #endregion

        #region IDeepClonable

        public Game DeepClone()
        {
            return new Game()
            {
                Id = this.Id,
                Created = this.Created,
                Started = this.Started,
                InVolley = this.InVolley,
                ScoreBlack = this.ScoreBlack,
                ScoreYellow = this.ScoreYellow,
                TeamBlack = this.TeamBlack.DeepClone(),
                TeamYellow = this.TeamYellow.DeepClone()
            };
        }

        #endregion

    }

    #region IEqualityComparer

    public class GameEqualityComparer : IEqualityComparer<Game>
    {
        public bool Equals(Game x, Game y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            // Compare by value equality
            var result =
                x.Id == y.Id &&
                x.Created == y.Created &&
                x.Started == y.Started &&
                x.InVolley == y.InVolley &&
                x.ScoreBlack == y.ScoreBlack &&
                x.ScoreYellow == y.ScoreYellow &&
                x.TeamBlack.SafeSequenceEqual(y.TeamBlack) &&
                x.TeamYellow.SafeSequenceEqual(y.TeamYellow);
            return result;
        }

        public int GetHashCode(Game obj)
        {
            // Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            return HashCode.Start
                .Hash(obj.Id)
                .Hash(obj.Created)
                .Hash(obj.Started)
                .Hash(obj.InVolley)
                .Hash(obj.ScoreBlack)
                .Hash(obj.ScoreYellow)
                .Hash(obj.TeamBlack)
                .Hash(obj.TeamYellow);
        }
    }

    #endregion

}
