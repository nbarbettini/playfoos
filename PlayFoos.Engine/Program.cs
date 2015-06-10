using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using NLog;
using Microsoft.Owin.Hosting;
using Topshelf.StructureMap;

namespace PlayFoos.Engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IDisposable selfHost = null;
            string url = @"http://localhost:8080/";

            // Initialize IOC container
            IOC.Initialize();

            HostFactory.Run(x =>
            {
                x.RunAsNetworkService();
                x.UseNLog();
                x.UseStructureMap(IOC.Container);

                x.Service<Engine>(s =>
                {
                    s.ConstructUsingStructureMap();

                    s.WhenStarted(e => {
                        selfHost = WebApp.Start<Startup>(url);
                        e.Start();
                    });

                    s.WhenStopped(e => {
                        e.Stop();
                        selfHost.Dispose();
                    });
                });

                x.SetDescription("PlayFoos Engine");
                x.SetDisplayName("PlayFoosEngine");
                x.SetServiceName("PlayFoosEngine");
            });
        }
    }
}
