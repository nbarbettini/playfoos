using Microsoft.AspNet.SignalR;
using NLog;
using PlayFoos.Core.Model;
using PlayFoos.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Engine.Workers
{
    public class GameStateWorker : Worker
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IHubContext _hubContext;
        private readonly IGameService _gameService;
        private readonly IGameArchiveService _gameArchiveService;
        private readonly IGameLogicService _gameLogicService;

        private Game _previousGameState = null;

        public GameStateWorker(IHubContext hubContext, IGameService gameService, IGameArchiveService gameArchiveService, IGameLogicService gameLogicService)
        {
            _hubContext = hubContext;
            _gameService = gameService;
            _gameArchiveService = gameArchiveService;
            _gameLogicService = gameLogicService;
        }

        protected override async Task<bool> UpdateInternalAsync()
        {
            var current = await _gameService.GetCurrentAsync();

            if (_gameLogicService.IsGameOver(current))
            {
                // archive

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

            return true;
        }
    }
}
