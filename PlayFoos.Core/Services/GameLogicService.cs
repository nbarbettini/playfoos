using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public class GameLogicService : IGameLogicService
    {
        public bool IsGameOver(Model.Game game)
        {
            if (game == null) return false;

            // "10, win by 2" reckoning
            if (game.ScoreBlack < 10 &&
                game.ScoreYellow < 10)
                return false;

            return ((game.ScoreBlack - game.ScoreYellow) > 1 ||
                (game.ScoreYellow - game.ScoreBlack) > 1);
        }
    }
}
