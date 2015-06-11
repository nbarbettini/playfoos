using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PlayFoos.Core
{
    public static class Config
    {
        public static string MongoConnection
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoConnection"];
            }
        }

        public static string MongoDatabase
        {
            get
            {
                return ConfigurationManager.AppSettings["MongoDatabase"];
            }
        }

        public static string EngineChannelUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["EngineChannelUrl"];
            }
        }
    }
}