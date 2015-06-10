using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public class GameLogicService : IGameLogicService
    {
        private readonly IRatingCalculatorService _ratingCalculatorService;

        public GameLogicService(IRatingCalculatorService ratingCalculatorService)
        {
            _ratingCalculatorService = ratingCalculatorService;
        }

        public bool ArePlayersValid(Model.Game game)
        {
            bool hasEnoughPlayers = (game.PlayersBlack.Count == 2 && game.PlayersYellow.Count == 2);
            if (!hasEnoughPlayers)
                return false;

            return true;
        }

        public bool IsGameOver(Model.Game game)
        {
            if (game == null)
                return false;

            bool anyScoreAboveTen = (game.ScoreBlack < 10 && game.ScoreYellow < 10);
            if (anyScoreAboveTen)
                return false;

            bool eitherTeamWonByTwo = 
                ((game.ScoreBlack - game.ScoreYellow) > 1 || (game.ScoreYellow - game.ScoreBlack) > 1);
            return eitherTeamWonByTwo;
        }

        public Model.GameCompleted CompleteGame(Model.Game game)
        {
            if (!IsGameOver(game))
                return null;

            var blackWon = (game.ScoreBlack > game.ScoreYellow);
            var endedAt = DateTime.Now;

            var completed = new Model.GameCompleted()
            {
                Id = game.Id,
                Created = game.Created,
                Started = game.Started.Value,
                Ended = endedAt,
                Duration = endedAt - game.Started.Value,
                ScoreBlack = game.ScoreBlack,
                ScoreYellow = game.ScoreYellow,
                BlackWon = blackWon
            };

            if (ArePlayersValid(game))
            {
                var newRatings = _ratingCalculatorService.CalculateRatingChange(game.PlayersBlack, game.PlayersYellow, blackWon);
                if (newRatings.IsValid())
                {
                    foreach (var player in game.PlayersBlack)
                    {
                        var historicalPlayer = new Model.PlayerHistorical(player);
                        historicalPlayer.EndRating = newRatings.ForPlayer(player.Id);
                        completed.PlayersBlack.Add(historicalPlayer);
                    }
                    foreach (var player in game.PlayersYellow)
                    {
                        var historicalPlayer = new Model.PlayerHistorical(player);
                        historicalPlayer.EndRating = newRatings.ForPlayer(player.Id);
                        completed.PlayersYellow.Add(historicalPlayer);
                    }
                }
            }

            return completed;
        }

        
    }
}
