using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using System.Timers;
using Microsoft.AspNet.SignalR;
using PlayFoos.Core.Model;
using PlayFoos.Core;
using PlayFoos.Core.Services;
using Nito.AsyncEx;
using PlayFoos.Engine.Workers;

namespace PlayFoos.Engine
{
    public class Engine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Timer _timer;

        private readonly IHubContext _hubContext;
        private readonly MongoContext _context;

        private readonly IGameService _gameService;
        private readonly IGameArchiveService _gameArchiveService;
        private readonly IGameLogicService _gameLogicService;

        // Workers
        private readonly List<Worker> _workers;

        public Engine()
        {
            // Connect to Mongo and services (TODO)
            _context = new MongoContext("mongodb://192.168.2.140", "PlayFoos");
            _gameService = new GameService(_context);
            _gameLogicService = new GameLogicService(new EloRatingCalculatorService());

            // Connect to SignalR hub
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();

            // Set up workers
            _workers = new List<Worker>() {
                new GameStateWorker(_hubContext, _gameService, _gameArchiveService, _gameLogicService)
            };

            // Set up timers
            _timer = new Timer(1000) { AutoReset = false };
            _timer.Elapsed += async (s, e) =>
            {
                await Update();
                _timer.Start();
            };

#if DEBUG
            // DEBUG: Watch the console
            Task.Run(async () =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true);
                        if (key.KeyChar == 'u')
                            await Update();
                    }
                }
            });
#endif
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

            // Will block if workers are currently updating
            _workers.ForEach(x => x.Dispose());
        }

        private async Task Update()
        {
            await Task.WhenAll(
                _workers.Select(x => x.UpdateAsync()));
        }
    }
}
