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

namespace PlayFoos.Engine
{
    public class Engine
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private readonly Timer _updateGameStateTimer;
        private readonly AsyncLock _updateGameStateLock = new AsyncLock();

        private readonly IHubContext _hubContext;
        private readonly MongoContext _context;

        private readonly IGameService _gameService;
        private readonly IGameLogicService _gameLogicService;
        private Game _previousGameState = null;
        
        public Engine()
        {
            // Connect to Mongo and services (TODO)
            _context = new MongoContext("mongodb://192.168.2.140", "PlayFoos");
            _gameService = new GameService(_context);
            _gameLogicService = new GameLogicService();

            // Connect to SignalR hub
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<NotifyHub>();

            // Set up timers
            _updateGameStateTimer = new Timer(500) { AutoReset = false };
            _updateGameStateTimer.Elapsed += async (s, e) =>
            {
                await UpdateGameState();
                _updateGameStateTimer.Start();
            };
        }

        public void Start()
        {
            logger.Info("Engine starting...");
            _updateGameStateTimer.Start();
        }

        public void Stop()
        {
            logger.Info("Engine stopping...");

            using (_updateGameStateLock.Lock())
            {
                _updateGameStateTimer.Stop();
            }
        }

        private async Task UpdateGameState()
        {
            var lockTimeout = new System.Threading.CancellationTokenSource(TimeSpan.FromMilliseconds(1000));

            try
            {
                using (await _updateGameStateLock.LockAsync(lockTimeout.Token))
                {
                    var current = await _gameService.GetCurrentAsync();

                    if (_gameLogicService.IsGameOver(current))
                    {
                        // Archive (TODO)
                        logger.Info("Game over!");
                        await _gameService.DeleteCurrentAsync();
                        current = null;
                    }

                    if (current != _previousGameState)
                    {
                        logger.Info("Game state changed!");
                        _hubContext.Clients.All.UpdateGameState(current);
                        _previousGameState = current;
                    }
                }
            }
            catch (OperationCanceledException) {
                Console.WriteLine("lock skipped");
            }

        }
    }
}
