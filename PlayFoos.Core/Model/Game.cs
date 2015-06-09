using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class Game : IEquatable<Game>, IDeepClonable<Game>
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Started { get; set; }

        public int ScoreBlack { get; set; }
        public int ScoreYellow { get; set; }

        public List<PlayerActive> PlayersBlack { get; set; }
        public List<PlayerActive> PlayersYellow { get; set; }

        // Default constructor
        public Game()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;

            PlayersBlack = new List<PlayerActive>();
            PlayersYellow = new List<PlayerActive>();
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
                ScoreBlack = this.ScoreBlack,
                ScoreYellow = this.ScoreYellow,
                PlayersBlack = this.PlayersBlack.DeepClone(),
                PlayersYellow = this.PlayersYellow.DeepClone()
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
                x.ScoreBlack == y.ScoreBlack &&
                x.ScoreYellow == y.ScoreYellow &&
                x.PlayersBlack.SafeSequenceEqual(y.PlayersBlack) &&
                x.PlayersYellow.SafeSequenceEqual(y.PlayersYellow);
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
                .Hash(obj.ScoreBlack)
                .Hash(obj.ScoreYellow)
                .Hash(obj.PlayersBlack)
                .Hash(obj.PlayersYellow);
        }
    }

    #endregion

}
