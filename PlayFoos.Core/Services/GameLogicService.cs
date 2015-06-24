using PlayFoos.Core.Objects;
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
        private readonly IClock _clock;

        public GameLogicService(IRatingCalculatorService ratingCalculatorService, IClock clock)
        {
            _ratingCalculatorService = ratingCalculatorService;
            _clock = clock;
        }

        public bool ArePlayersValid(Model.Game game)
        {
            bool hasEnoughPlayers = (game.TeamBlack.Count == 2 && game.TeamYellow.Count == 2);
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
            var endedAt = _clock.Now;

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
                var newRatings = _ratingCalculatorService.CalculateRatingChange(game.TeamBlack, game.TeamYellow, blackWon);
                if (newRatings.IsValid())
                {
                    foreach (var player in game.TeamBlack)
                    {
                        var historicalPlayer = new Model.PlayerHistorical(player);
                        historicalPlayer.EndRating = newRatings.ForPlayer(player.Id);
                        completed.PlayersBlack.Add(historicalPlayer);
                    }
                    foreach (var player in game.TeamYellow)
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
