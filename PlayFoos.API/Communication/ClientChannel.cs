using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlayFoos.API.Communication
{
    public class ClientChannel : IClientChannel
    {
        private readonly IHubContext _hubContext;

        public ClientChannel()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();
        }

        public async Task BroadcastGameState(Core.Model.Game game)
        {
            await _hubContext.Clients.All.updateGameState(game);
        }
    }
}