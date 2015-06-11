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
using PlayFoos.Core.Context;

namespace PlayFoos.Engine
{
    public class Engine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly Timer _timer;
        private readonly System.Threading.CancellationTokenSource _lockImmediately;
        protected readonly AsyncLock _lock = new AsyncLock();

        private readonly IHubContext _hubContext;
        private readonly IMongoContext _mongoContext;

        private readonly IGameService _gameService;
        private readonly IGameArchiveService _gameArchiveService;
        private readonly IGameLogicService _gameLogicService;

        // Workers
        private readonly List<Worker> _workers;

        public Engine(IHubContext hubContext, 
            IMongoContext mongoContext, 
            IGameService gameService, 
            IGameArchiveService gameArchiveService,
            IGameLogicService gameLogicSerivce)
        {
            // Connect to Mongo and services (TODO)
            _hubContext = hubContext;
            _mongoContext = mongoContext;
            _gameService = gameService;
            _gameArchiveService = gameArchiveService;
            _gameLogicService = gameLogicSerivce;

            // Set up workers
            _workers = new List<Worker>() {
                new GameStateWorker(_hubContext, _gameService, _gameArchiveService, _gameLogicService)
            };

            // Set up callback on Hub
            NotifyHub.UpdateCallback = Update;

            // Set up timer
            _lockImmediately = new System.Threading.CancellationTokenSource();
            _lockImmediately.Cancel();

            _timer = new Timer(2000) { AutoReset = false };
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

        private async Task Update()
        {
            try
            {
                using (await _lock.LockAsync(_lockImmediately.Token))
                {
                    await Task.WhenAll(
                        _workers.Select(x => x.UpdateAsync()));
                }
            }
            catch (OperationCanceledException) { }
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
    }
}
