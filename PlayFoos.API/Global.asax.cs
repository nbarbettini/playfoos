using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using PlayFoos.API.Hubs;
using PlayFoos.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace PlayFoos.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            // Set up SignalR client for service communication
            IHubProxy _hub;
            var connection = new HubConnection(@"http://localhost:8080/");
            _hub = connection.CreateHubProxy("NotifyHub");
            connection.Start();

            IHubContext _hubContext;
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<GameStateHub>();

            throw new NotImplementedException();
            //_hub.On<GameStateDto>("UpdateGameState", state => {
            //    // Push to all connected clients
            //    _hubContext.Clients.All.UpdateGameState(state);
            //});
        }
    }
}
