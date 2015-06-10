using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface IRatingCalculatorService
    {
        RatingUpdateResultDto CalculateRatingChange(
            IEnumerable<Model.PlayerActive> firstTeam,
            IEnumerable<Model.PlayerActive> secondTeam,
            bool firstTeamWon);
    }
}
