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

            // SignalR for backend communication with engine
            EngineChannel.Initialize(Core.Config.EngineChannelUrl);
            EngineChannel.Start();
        }
    }
}
