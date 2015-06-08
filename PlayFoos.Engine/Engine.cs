using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Timers;
using Microsoft.AspNet.SignalR;
using PlayFoos.Core.Model;

namespace PlayFoos.Engine
{
    public class Engine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private readonly Timer _timer;
        private readonly IHubContext _hubContext;
        private int _stateCount;

        public Engine()
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();

            _timer = new Timer(5000) { AutoReset = true };
            _timer.Elapsed += (s, e) =>
            {
                var state = new GameStateDto()
                {
                    Id = ++_stateCount,
                    Started = false,
                    StartTime = DateTime.Now.AddSeconds(5)
                };
                _hubContext.Clients.All.UpdateGameState(state);
                logger.Info("Game {0} {1} at {2}", state.Id, state.Started ? "started" : "starting", state.StartTime);
            };
        }

        public void Start()
        {
            logger.Info("Engine starting...");
            _timer.Start();
        }

        public void Stop()
        {
            logger.Info("Engine stopping...");
            _timer.Stop();
        }
    }
}
