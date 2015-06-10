using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayFoos.Core.Services
{
    public interface IGameLogicService
    {
        bool ArePlayersValid(Model.Game game);
        bool IsGameOver(Model.Game game);

        Model.GameCompleted CompleteGame(Model.Game game);
    }
}
