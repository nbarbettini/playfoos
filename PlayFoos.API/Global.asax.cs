using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;
using PlayFoos.API.DependencyResolution;
using PlayFoos.API.Communication;
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

            // Set up IOC container
            IOC.Initialize();
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(IOC.Container);

            // Set up dependency for NotifyHub
            NotifyHub.GameServiceInstance = IOC.Container.GetInstance<Core.Services.IGameService>();

            // Start SignalR communication
            var engineChannel = IOC.Container.GetInstance<IEngineChannel>();
            engineChannel.StartIfNecessary();
        }
    }
}
