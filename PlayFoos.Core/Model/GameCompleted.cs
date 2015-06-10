using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class GameCompleted : IEquatable<GameCompleted>, IDeepClonable<GameCompleted>
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("createdAt")]
        public DateTime Created { get; set; }

        [BsonElement("startAt")]
        public DateTime Started { get; set; }

        [BsonElement("doneAt")]
        public DateTime Ended { get; set; }

        [BsonElement("len")]
        public TimeSpan Duration { get; set; }

        [BsonElement("bScore")]
        public int ScoreBlack { get; set; }

        [BsonElement("yScore")]
        public int ScoreYellow { get; set; }

        [BsonElement("bWins")]
        public bool BlackWon { get; set; }

        [BsonElement("bPlayers")]
        public List<PlayerHistorical> PlayersBlack { get; set; }

        [BsonElement("yPlayers")]
        public List<PlayerHistorical> PlayersYellow { get; set; }

        // Default constructor
        public GameCompleted()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;

            PlayersBlack = new List<PlayerHistorical>();
            PlayersYellow = new List<PlayerHistorical>();
        }

        #region IEquatable

        public bool Equals(GameCompleted other)
        {
            return Equals((object)other);
        }

        public override bool Equals(object obj)
        {
            var other = obj as GameCompleted;
            if (obj == null)
                return false;

            return new GameCompletedEqualityComparer().Equals(this, other);
        }

        public override int GetHashCode()
        {
            return new GameCompletedEqualityComparer().GetHashCode(this);
        }

        public static bool operator ==(GameCompleted x, GameCompleted y)
        {
            return new GameCompletedEqualityComparer().Equals(x, y);
        }

        public static bool operator !=(GameCompleted x, GameCompleted y)
        {
            return !(x == y);
        }

        #endregion

        #region IDeepClonable

        public GameCompleted DeepClone()
        {
            return new GameCompleted()
            {
                Id = Id,
                Created = Created,
                Started = Started,
                Ended = Ended,
                Duration = Duration,
                ScoreBlack = ScoreBlack,
                ScoreYellow = ScoreYellow,
                BlackWon = BlackWon,
                PlayersBlack = PlayersBlack.DeepClone(),
                PlayersYellow = PlayersYellow.DeepClone()
            };
        }

        #endregion

    }

    #region IEqualityComparer

    public class GameCompletedEqualityComparer : IEqualityComparer<GameCompleted>
    {
        public bool Equals(GameCompleted x, GameCompleted y)
        {
            if (object.ReferenceEquals(x, y)) return true;

            if (object.ReferenceEquals(x, null) || object.ReferenceEquals(y, null))
                return false;

            // Compare by value equality
            var result =
                x.Id == y.Id &&
                x.Created == y.Created &&
                x.Started == y.Started &&
                x.Duration == y.Duration &&
                x.Ended == y.Ended &&
                x.ScoreBlack == y.ScoreBlack &&
                x.ScoreYellow == y.ScoreYellow &&
                x.BlackWon == y.BlackWon &&
                x.PlayersBlack.SafeSequenceEqual(y.PlayersBlack) &&
                x.PlayersYellow.SafeSequenceEqual(y.PlayersYellow);
            return result;
        }

        public int GetHashCode(GameCompleted obj)
        {
            // Check whether the object is null 
            if (Object.ReferenceEquals(obj, null)) return 0;

            return HashCode.Start
                .Hash(obj.Id)
                .Hash(obj.Created)
                .Hash(obj.Started)
                .Hash(obj.Duration)
                .Hash(obj.Ended)
                .Hash(obj.ScoreBlack)
                .Hash(obj.ScoreYellow)
                .Hash(obj.BlackWon)
                .Hash(obj.PlayersBlack)
                .Hash(obj.PlayersYellow);
        }
    }

    #endregion

}
