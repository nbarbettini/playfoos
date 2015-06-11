using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayFoos.API.Communication
{
    public static class ClientChannel
    {
        private static readonly Lazy<IHubContext> _context =
            new Lazy<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<NotifyHub>());

        public static IHubContext Hub
        {
            get { return _context.Value; }
        }
    }
}