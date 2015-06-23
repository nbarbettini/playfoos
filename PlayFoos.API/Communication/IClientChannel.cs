using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.API.Communication
{
    public interface IClientChannel
    {
        Task BroadcastGameState(Core.Model.Game game);
    }
}
