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
        public RatingUpdateResultDto CalculateRatingChange(IEnumerable<PlayerActive> firstTeam, IEnumerable<PlayerActive> secondTeam, bool firstTeamWon)
        {
            var output = new RatingUpdateResultDto();

            if (firstTeam.Count() != 2 || secondTeam.Count() != 2)
                return output;

            // Update ratings for each player vs. both players on other team
            int adjustment = 0;
            foreach (var player in firstTeam) {
                adjustment = 0;
                adjustment += CalculateAdjustmentForPair(player, secondTeam.ElementAt(0), firstTeamWon).Item1;
                adjustment += CalculateAdjustmentForPair(player, secondTeam.ElementAt(1), firstTeamWon).Item1;
                output.Results.Add(new Tuple<Guid,int>(player.Id, player.Rating + adjustment));
            }
            foreach (var player in secondTeam) {
                adjustment = 0;
                adjustment += CalculateAdjustmentForPair(player, firstTeam.ElementAt(0), !firstTeamWon).Item1;
                adjustment += CalculateAdjustmentForPair(player, firstTeam.ElementAt(1), !firstTeamWon).Item1;
                output.Results.Add(new Tuple<Guid,int>(player.Id, player.Rating + adjustment));
            }

            return output;
        }

        internal Tuple<int, int> CalculateAdjustmentForPair(PlayerActive player1, PlayerActive player2, bool player1Won)
        {
            double currentRating1 = player1.Rating;
            double currentRating2 = player2.Rating;

            double finalResult1;
            double finalResult2;

            double e;

            if (player1Won)
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating2 - currentRating1) / 400))) * 120);
                finalResult1 = e;
                finalResult2 = -e;
            }
            else
            {
                e = 120 - Math.Round(1 / (1 + Math.Pow(10, ((currentRating1 - currentRating2) / 400))) * 120);
                finalResult1 = -e;
                finalResult2 = e;
            }

            return new Tuple<int, int>((int)finalResult1, (int)finalResult2);
        }
    }
}
