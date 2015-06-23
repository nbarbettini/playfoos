using Microsoft.AspNet.SignalR;
using PlayFoos.API.Communication;
using PlayFoos.Core.Context;
using PlayFoos.Core.Objects;
using PlayFoos.Core.Services;
using StructureMap;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PlayFoos.API.DependencyResolution
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

                x.For<IClock>()
                    .Use<NowClock>();

                // SignalR channels
                x.For<IClientChannel>()
                    .Use<ClientChannel>()
                    .Singleton();
                x.For<IEngineChannel>()
                    .Use<EngineChannel>()
                    .Ctor<string>("url").Is(Core.Config.EngineChannelUrl);

                // Mongo context
                x.For<IMongoContext>()
                    .Use<MongoContext>()
                    .Singleton()
                    .Ctor<string>("connectionString").Is(PlayFoos.Core.Config.MongoConnection)
                    .Ctor<string>("database").Is(PlayFoos.Core.Config.MongoDatabase);

                // Elo rating calculator
                x.For<IRatingCalculatorService>()
                    .Use<EloRatingCalculatorService>();
            });
        }
    }
}