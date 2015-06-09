using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayFoos.Core.Services
{
    public interface IGameLogicService
    {
        bool IsGameOver(Model.Game game);
    }
}
