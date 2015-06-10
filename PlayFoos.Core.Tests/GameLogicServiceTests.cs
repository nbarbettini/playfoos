using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFoos.Core.Model;
using PlayFoos.Core.Services;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;

namespace PlayFoos.Core.Tests
{
    [TestClass]
    public class GameLogicServiceTests
    {
        private GameLogicService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new GameLogicService(new EloRatingCalculatorService());
        }

        #region IsGameOver

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_game_is_null_GameOver_should_return_null()
        {
            Game game = null;

            Assert.IsFalse(_service.IsGameOver(game));
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_game_is_not_over()
        {
            var game = new Game();
            game.ScoreBlack = 9;

            Assert.IsFalse(_service.IsGameOver(game));
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_game_is_over()
        {
            var game = new Game();
            game.ScoreYellow = 10;
            game.ScoreBlack = 8;

            Assert.IsTrue(_service.IsGameOver(game));
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_game_is_in_win_by_2()
        {
            var game = new Game();
            game.ScoreBlack = 10;
            game.ScoreYellow = 9;

            Assert.IsFalse(_service.IsGameOver(game));
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_win_by_2_game_is_over()
        {
            var game = new Game();
            game.ScoreBlack = 11;
            game.ScoreYellow = 9;

            Assert.IsTrue(_service.IsGameOver(game));
        }

        #endregion

        #region CompleteGame

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_completing_game_with_no_players()
        {
            using (ShimsContext.Create())
            {
                var fakeNow = DateTime.Parse("2015/06/01 5:00PM");
                System.Fakes.ShimDateTime.NowGet = () => fakeNow;

                var game = new Model.Game()
                {
                    ScoreBlack = 11,
                    ScoreYellow = 9,
                    Started = DateTime.Now.AddMinutes(-1) // started 1 minute ago
                };

                var completed = _service.CompleteGame(game);

                Assert.IsNotNull(completed);
                Assert.AreEqual(game.Id, completed.Id);
                Assert.AreEqual(game.Created, completed.Created);
                Assert.AreEqual(fakeNow, completed.Ended);
                Assert.AreEqual(TimeSpan.FromMinutes(1), completed.Duration);
                Assert.AreEqual(game.ScoreBlack, completed.ScoreBlack);
                Assert.AreEqual(game.ScoreYellow, completed.ScoreYellow);
                Assert.IsTrue(completed.BlackWon);
                Assert.IsFalse(completed.PlayersBlack.Any());
                Assert.IsFalse(completed.PlayersYellow.Any());
            }
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_completing_game_with_players()
        {
            using (ShimsContext.Create())
            {
                var fakeNow = DateTime.Parse("2015/06/01 5:00PM");
                System.Fakes.ShimDateTime.NowGet = () => fakeNow;

                var game = new Model.Game()
                {
                    ScoreBlack = 9,
                    ScoreYellow = 11,
                    Started = DateTime.Now.AddMinutes(-1), // started 1 minute ago
                    PlayersBlack = new List<PlayerActive>()
                    {
                        new PlayerActive() { Name = "Player1", Rating = 2000},
                        new PlayerActive() { Name = "Player2", Rating = 1900},
                    },
                    PlayersYellow = new List<PlayerActive>()
                    {
                        new PlayerActive() { Name = "Player3", Rating = 1500},
                        new PlayerActive() { Name = "Player4", Rating = 1300},
                    }
                };

                var completed = _service.CompleteGame(game);

                Assert.IsNotNull(completed);
                Assert.AreEqual(game.Id, completed.Id);
                Assert.AreEqual(game.Created, completed.Created);
                Assert.AreEqual(fakeNow, completed.Ended);
                Assert.AreEqual(TimeSpan.FromMinutes(1), completed.Duration);
                Assert.AreEqual(game.ScoreBlack, completed.ScoreBlack);
                Assert.AreEqual(game.ScoreYellow, completed.ScoreYellow);
                Assert.IsFalse(completed.BlackWon);
                Assert.IsTrue(completed.PlayersBlack.Any());
                Assert.IsTrue(completed.PlayersYellow.Any());

                // Upset! Expected ELO ranking:
                //  Player1: (-114 -118) => 1768
                //  Player2: (-109 -116) => 1675
                //  Player3: (+114 +109) => 1723
                //  Player4: (+118 +116) => 1534
                Assert.AreEqual(2000, completed.PlayersBlack.Where(x => x.Name == "Player1").Single().StartRating);
                Assert.AreEqual(1768, completed.PlayersBlack.Where(x => x.Name == "Player1").Single().EndRating);

                Assert.AreEqual(1900, completed.PlayersBlack.Where(x => x.Name == "Player2").Single().StartRating);
                Assert.AreEqual(1675, completed.PlayersBlack.Where(x => x.Name == "Player2").Single().EndRating);

                Assert.AreEqual(1500, completed.PlayersYellow.Where(x => x.Name == "Player3").Single().StartRating);
                Assert.AreEqual(1723, completed.PlayersYellow.Where(x => x.Name == "Player3").Single().EndRating);

                Assert.AreEqual(1300, completed.PlayersYellow.Where(x => x.Name == "Player4").Single().StartRating);
                Assert.AreEqual(1534, completed.PlayersYellow.Where(x => x.Name == "Player4").Single().EndRating);
            }
        }

        #endregion

    }
}
