﻿using PlayFoos.Core.Context;
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

                // Mongo context
                x.For<IMongoContext>()
                    .Use<MongoContext>()
                    .Singleton()
                    .Ctor<string>("connectionString").Is(PlayFoos.Core.Config.MongoConnection)
                    .Ctor<string>("database").Is(PlayFoos.Core.Config.MongoDatabase);
            });
        }
    }
}