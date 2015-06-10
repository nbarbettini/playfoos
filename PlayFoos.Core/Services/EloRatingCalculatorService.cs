using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFoos.Core.Model;

namespace PlayFoos.Core.Services
{
    public class EloRatingCalculatorService : IRatingCalculatorService
    {
        public List<Tuple<Guid, int?>> GetNewRatings(IEnumerable<PlayerActive> firstTeam, IEnumerable<PlayerActive> secondTeam, bool firstTeamWon)
        {
            var result = new List<Tuple<Guid, int?>>();

            if (!firstTeam.Any() || !secondTeam.Any())
                return result;

            result.AddRange(
                firstTeam.Select(x => new Tuple<Guid, int?>(x.Id, x.Rating + (firstTeamWon ? 100 : 0))));
            result.AddRange(
                secondTeam.Select(x => new Tuple<Guid, int?>(x.Id, x.Rating + (!firstTeamWon ? 100 : 0))));

            return result;
        }

        public int? GetNewRating(Func<PlayerActive, bool> selector, IEnumerable<PlayerActive> firstTeam, IEnumerable<PlayerActive> secondTeam, bool firstTeamWon)
        {
            var allResults = GetNewRatings(firstTeam, secondTeam, firstTeamWon);
            var player = firstTeam.Concat(secondTeam)
                .Where(selector)
                .SingleOrDefault();

            if (player == null)
                return null;

            var result = allResults.Where(x => x.Item1 == player.Id).SingleOrDefault();

            if (result == null)
                return null;

            return result.Item2;
        }

        internal void UpdateSingleRating(PlayerActive player1, PlayerActive player2, bool player1Won)
        {
            double currentRating1 = player1.Rating;
            double currentRating2 = player2.Rating;

            double finalResult1;
            double finalResult2;

            double e;

            if (player1Won)
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / 400))) * 120);
                finalResult1 = currentRating1 + e;
                finalResult2 = currentRating2 - e;
            }
            else
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / 400))) * 120);
                finalResult1 = currentRating1 - e;
                finalResult2 = currentRating2 + e;
            }

            player1.Rating = (int)finalResult1;
            player2.Rating = (int)finalResult2;
        }
    }
}
