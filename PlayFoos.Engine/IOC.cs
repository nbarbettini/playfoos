using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;
using StructureMap.Graph;
using PlayFoos.Core.Context;
using Microsoft.AspNet.SignalR;
using PlayFoos.Core.Services;

namespace PlayFoos.Engine
{
    public static class IOC
    {
        public static IContainer Container;

        public static void Initialize()
        {
            Container = new Container(x =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.AssemblyContainingType<IGameService>();
                    scan.WithDefaultConventions();
                });

                // Mongo context
                x.For<IMongoContext>()
                    .Use<MongoContext>()
                    .Singleton()
                    .Ctor<string>("connectionString").Is(PlayFoos.Core.Config.MongoConnection)
                    .Ctor<string>("database").Is(PlayFoos.Core.Config.MongoDatabase);

                // SignalR hub
                x.For<IHubContext>()
                    .Use(GlobalHost.ConnectionManager.GetHubContext<NotifyHub>())
                    .Singleton();

                // Elo rating calculator
                x.For<IRatingCalculatorService>()
                    .Use<EloRatingCalculatorService>();
            });
        }
    }
}
