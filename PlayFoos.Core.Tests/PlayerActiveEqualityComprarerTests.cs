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
    public class PlayerActiveEqualityComparerTests
    {
        [TestMethod]
        [TestCategory("Core.PlayerActiveEqualityComparer")]
        public void When_comparing_null_players()
        {
            PlayerActive player1 = null, player2 = null;

            Assert.IsTrue(player1 == player2);
        }

        [TestMethod]
        [TestCategory("Core.PlayerActiveEqualityComparer")]
        public void When_comparing_new_players()
        {
            PlayerActive player = new PlayerActive();

            Assert.IsFalse(player == new PlayerActive());
            Assert.IsFalse(player.Equals(new PlayerActive()));
        }

        [TestMethod]
        [TestCategory("Core.PlayerActiveEqualityComparer")]
        public void When_comparing_equal_players()
        {
            var id = Guid.NewGuid();
            var player1 = new PlayerActive()
            {
                Id = id,
                Name = "John Foobar",
                Rating = 100
            };
            var player2 = new PlayerActive()
            {
                Id = id,
                Name = "John Foobar",
                Rating = 100
            };

            Assert.AreEqual(player1, player2);
            Assert.IsTrue(player1 == player2);
            Assert.IsTrue(player1.Equals(player2));
        }

        [TestMethod]
        [TestCategory("Core.PlayerActiveEqualityComparer")]
        public void When_comparing_unequal_players()
        {
            var id = Guid.NewGuid();
            var player1 = new PlayerActive()
            {
                Id = id,
                Name = "John Foobar",
                Rating = 100
            };
            var player2 = new PlayerActive()
            {
                Id = id,
                Name = "Jill Foobar",
                Rating = 100
            };

            Assert.AreNotEqual(player1, player2);
            Assert.IsFalse(player1 == player2);
            Assert.IsFalse(player1.Equals(player2));
        }
    }
}
