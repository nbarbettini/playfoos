using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public class RatingUpdateResultDto
    {
        public List<Tuple<Guid, int>> Results { get; set; }

        public RatingUpdateResultDto()
        {
            Results = new List<Tuple<Guid, int>>();
        }

        public int ForPlayer(Guid id)
        {
            var player = Results
                .Where(x => x.Item1 == id)
                .SingleOrDefault();

            if (player == null)
                throw new KeyNotFoundException();

            return player.Item2;
        }

        public bool IsValid()
        {
            bool validCount = (Results.DistinctBy(x => x.Item1).Count() == 4);
            return validCount;
        }
    }
}
