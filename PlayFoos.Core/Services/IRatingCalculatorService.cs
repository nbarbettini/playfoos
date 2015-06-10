using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface IRatingCalculatorService
    {
        List<Tuple<Guid, int?>> GetNewRatings(
            IEnumerable<Model.PlayerActive> firstTeam,
            IEnumerable<Model.PlayerActive> secondTeam,
            bool firstTeamWon);

        int? GetNewRating(Func<Model.PlayerActive, bool> selector,
            IEnumerable<Model.PlayerActive> firstTeam,
            IEnumerable<Model.PlayerActive> secondTeam,
            bool firstTeamWon);
    }
}
