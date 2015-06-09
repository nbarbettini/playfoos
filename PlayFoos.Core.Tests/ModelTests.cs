using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFoos.Core.Model;

namespace PlayFoos.Core.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        [TestCategory("Core.Model")]
        public void When_cloning_PlayerActive()
        {
            var player = new PlayerActive()
            {
                Id = Guid.NewGuid(),
                Name = "Foo Barbaz",
                Rating = 1670
            };

            var copy = player.DeepClone();

            Assert.AreEqual(player, copy);
        }

        [TestMethod]
        [TestCategory("Core.Model")]
        public void When_cloning_Game()
        {
            var game = new Game()
            {
                PlayersBlack = new List<PlayerActive>() { new PlayerActive() { Name = "Jack" } },
                Started = DateTime.Now.AddHours(1)
            };

            var copy = game.DeepClone();

            Assert.AreEqual(game, copy);
        }
    }
}
