using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlayFoos.Core.Model;
using PlayFoos.Core.Services;

namespace PlayFoos.Core.Tests
{
    [TestClass]
    public class EloRatingCalculatorServiceTests
    {
        private EloRatingCalculatorService _service;
        private List<PlayerActive> standardFirstTeam;
        private List<PlayerActive> standardSecondTeam;

        #region Setup

        [TestInitialize]
        public void Setup()
        {
            _service = new EloRatingCalculatorService();

            standardFirstTeam = new List<PlayerActive>();
            standardFirstTeam.Add(new PlayerActive()
            {
                Id = Guid.NewGuid(),
                Name = "Jon Snow",
                Rating = 1500
            });
            standardFirstTeam.Add(new PlayerActive()
            {
                Id = Guid.NewGuid(),
                Name = "Arya Stark",
                Rating = 1500
            });

            standardSecondTeam = new List<PlayerActive>();
            standardSecondTeam.Add(new PlayerActive()
            {
                Id = Guid.NewGuid(),
                Name = "Gregor Clegane",
                Rating = 1500
            });
            standardSecondTeam.Add(new PlayerActive()
            {
                Id = Guid.NewGuid(),
                Name = "Sandor Clegane",
                Rating = 1500
            });

        }

        #endregion

        #region Simple/logic tests
        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void Ensure_method_fails_for_incorrect_number_of_players()
        {
            var newRatings = _service.GetNewRatings(standardFirstTeam, new List<PlayerActive>(), true);

            Assert.IsFalse(newRatings.Any());
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void Ensure_tuple_order_is_correct()
        {
            var newRatings = _service.GetNewRatings(standardFirstTeam, standardSecondTeam, true);

            // [0, 1], [0, 1] => [0, 1, 2, 3]
            Assert.AreEqual(standardFirstTeam.ElementAt(0).Id, newRatings.ElementAt(0).Item1);
            Assert.AreEqual(standardFirstTeam.ElementAt(1).Id, newRatings.ElementAt(1).Item1);
            Assert.AreEqual(standardSecondTeam.ElementAt(0).Id, newRatings.ElementAt(2).Item1);
            Assert.AreEqual(standardSecondTeam.ElementAt(1).Id, newRatings.ElementAt(3).Item1);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_first_team_wins()
        {
            var newRatings = _service.GetNewRatings(standardFirstTeam, standardSecondTeam, true);

            Assert.AreEqual(1600, newRatings.ElementAt(0).Item2);
            Assert.AreEqual(1600, newRatings.ElementAt(1).Item2);
            Assert.AreEqual(1500, newRatings.ElementAt(2).Item2);
            Assert.AreEqual(1500, newRatings.ElementAt(3).Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_second_team_wins()
        {
            var newRatings = _service.GetNewRatings(standardFirstTeam, standardSecondTeam, false);

            Assert.AreEqual(1500, newRatings.ElementAt(0).Item2);
            Assert.AreEqual(1500, newRatings.ElementAt(1).Item2);
            Assert.AreEqual(1600, newRatings.ElementAt(2).Item2);
            Assert.AreEqual(1600, newRatings.ElementAt(3).Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_first_team_wins_selecting_single_player()
        {
            var player2 = standardFirstTeam.ElementAt(1);
            player2.Rating = 1501;

            var newRatingForPlayer2 = _service.GetNewRating(x => x.Id == standardFirstTeam.ElementAt(1).Id,
                standardFirstTeam, standardSecondTeam, true);

            Assert.AreEqual(1601, newRatingForPlayer2);
        }

        #endregion

        #region Elo rating math tests

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_new_players_and_player1_won()
        {
            var player1 = new PlayerActive() { Rating = 1500 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Two new players (1500), +/- 60 points with k=120

            _service.UpdateSingleRating(player1, player2, true);

            Assert.AreEqual(1560, player1.Rating);
            Assert.AreEqual(1440, player2.Rating);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_new_players_and_player2_won()
        {
            var player1 = new PlayerActive() { Rating = 1500 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Two new players (1500), +/- 60 points with k=120

            _service.UpdateSingleRating(player1, player2, false);

            Assert.AreEqual(1560, player2.Rating);
            Assert.AreEqual(1440, player1.Rating);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_overmatch_and_player1_won()
        {
            var player1 = new PlayerActive() { Rating = 2000 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Player1 gains 6 or loses 114 (k=120)
            // Player2 gains 114 or loses 6 (k=120)

            _service.UpdateSingleRating(player1, player2, true);

            Assert.AreEqual(2006, player1.Rating);
            Assert.AreEqual(1494, player2.Rating);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_undermatch_and_player1_won()
        {
            var player1 = new PlayerActive() { Rating = 900 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Player1 gains 116 or loses 4 (k=120)
            // Player2 gains 4 or loses 116 (k=120)

            _service.UpdateSingleRating(player1, player2, true);

            Assert.AreEqual(1016, player1.Rating);
            Assert.AreEqual(1384, player2.Rating);
        }

        #endregion


    }
}
