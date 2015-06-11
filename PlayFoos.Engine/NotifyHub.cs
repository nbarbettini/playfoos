using Microsoft.AspNet.SignalR;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Engine
{
    public class NotifyHub : Hub
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static Func<Task> UpdateCallback { get; set; }
        public async Task Update()
        {
            if (UpdateCallback != null)
                UpdateCallback();
        }
    }
}
