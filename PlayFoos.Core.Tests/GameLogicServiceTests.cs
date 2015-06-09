using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFoos.Core.Model;
using PlayFoos.Core.Services;

namespace PlayFoos.Core.Tests
{
    [TestClass]
    public class GameLogicServiceTests
    {
        private GameLogicService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new GameLogicService();
        }

        [TestMethod]
        [TestCategory("Core.GameLogicService")]
        public void When_game_is_null()
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
    }
}
