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
    public class GameEqualityComparerTests
    {
        [TestMethod]
        [TestCategory("Core.GameEqualityComparer")]
        public void When_comparing_null_games()
        {
            Game game1 = null, game2 = null;

            Assert.IsTrue(game1 == game2);
        }

        [TestMethod]
        [TestCategory("Core.GameEqualityComparer")]
        public void When_comparing_new_games()
        {
            Game game = new Game();

            Assert.IsFalse(game == new Game());
            Assert.IsFalse(game.Equals(new Game()));
        }


    }
}
