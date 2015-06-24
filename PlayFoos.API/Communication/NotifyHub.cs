using Microsoft.AspNet.SignalR;
using PlayFoos.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PlayFoos.API.Communication
{
    public class NotifyHub : Hub
    {
        public static IGameService GameServiceInstance { get; set; }

        public NotifyHub() { }

        public override async Task OnConnected()
        {
            if (GameServiceInstance != null)
                Clients.Caller.updateGameState(await GameServiceInstance.GetCurrentAsync());

            await base.OnConnected();
        }
    }
}