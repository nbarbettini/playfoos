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
        public void Should_return_empty_for_invalid_number_of_players()
        {
            var calculated = _service.CalculateRatingChange(standardFirstTeam, new List<PlayerActive>(), true);

            Assert.IsFalse(calculated.Results.Any());
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void Ensure_tuple_order_is_correct()
        {
            var calculated = _service.CalculateRatingChange(standardFirstTeam, standardSecondTeam, true);

            // [0, 1], [0, 1] => [0, 1, 2, 3]
            Assert.AreEqual(standardFirstTeam.ElementAt(0).Id, calculated.Results.ElementAt(0).Item1);
            Assert.AreEqual(standardFirstTeam.ElementAt(1).Id, calculated.Results.ElementAt(1).Item1);
            Assert.AreEqual(standardSecondTeam.ElementAt(0).Id, calculated.Results.ElementAt(2).Item1);
            Assert.AreEqual(standardSecondTeam.ElementAt(1).Id, calculated.Results.ElementAt(3).Item1);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_first_team_wins()
        {
            var calculated = _service.CalculateRatingChange(standardFirstTeam, standardSecondTeam, true);

            Assert.AreEqual(1620, calculated.Results.ElementAt(0).Item2);
            Assert.AreEqual(1620, calculated.Results.ElementAt(1).Item2);
            Assert.AreEqual(1380, calculated.Results.ElementAt(2).Item2);
            Assert.AreEqual(1380, calculated.Results.ElementAt(3).Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_second_team_wins()
        {
            var calculated = _service.CalculateRatingChange(standardFirstTeam, standardSecondTeam, false);

            Assert.AreEqual(1380, calculated.Results.ElementAt(0).Item2);
            Assert.AreEqual(1380, calculated.Results.ElementAt(1).Item2);
            Assert.AreEqual(1620, calculated.Results.ElementAt(2).Item2);
            Assert.AreEqual(1620, calculated.Results.ElementAt(3).Item2);
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

            var result = _service.CalculateAdjustmentForPair(player1, player2, true);

            Assert.AreEqual(60, result.Item1);
            Assert.AreEqual(-60, result.Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_new_players_and_player2_won()
        {
            var player1 = new PlayerActive() { Rating = 1500 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Two new players (1500), +/- 60 points with k=120

            var result = _service.CalculateAdjustmentForPair(player1, player2, false);

            Assert.AreEqual(-60, result.Item1);
            Assert.AreEqual(60, result.Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_overmatch_and_player1_won()
        {
            var player1 = new PlayerActive() { Rating = 2000 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Player1 gains 6 or loses 114 (k=120)
            // Player2 gains 114 or loses 6 (k=120)

            var result = _service.CalculateAdjustmentForPair(player1, player2, true);

            Assert.AreEqual(6, result.Item1);
            Assert.AreEqual(-6, result.Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_updating_single_rating_undermatch_and_player1_won()
        {
            var player1 = new PlayerActive() { Rating = 900 };
            var player2 = new PlayerActive() { Rating = 1500 };
            // Player1 gains 116 or loses 4 (k=120)
            // Player2 gains 4 or loses 116 (k=120)

            var result = _service.CalculateAdjustmentForPair(player1, player2, true);

            Assert.AreEqual(116, result.Item1);
            Assert.AreEqual(-116, result.Item2);
        }

        [TestMethod]
        [TestCategory("Core.EloRatingCalculatorService")]
        public void When_calculating_mixed_team_results()
        {
            standardFirstTeam.ElementAt(0).Rating = 1780;
            standardFirstTeam.ElementAt(1).Rating = 1300;
            standardSecondTeam.ElementAt(0).Rating = 2100;
            standardSecondTeam.ElementAt(1).Rating = 1920;

            // An underdog team beats a champion team!
            // Expected results:
            //  Team1 Player1: 1780 -> (+104 +83) = 1967
            //  Team1 Player2: 1300 -> (+119 +117) = 1536
            //  Team2 Player1: 2100 -> (-104 -119) = 1877
            //  Team2 Player2: 1920 -> (-83 -117) = 1720

            var output = _service.CalculateRatingChange(standardFirstTeam, standardSecondTeam, true);

            Assert.AreEqual(1967, output.Results.ElementAt(0).Item2);
            Assert.AreEqual(1536, output.Results.ElementAt(1).Item2);
            Assert.AreEqual(1877, output.Results.ElementAt(2).Item2);
            Assert.AreEqual(1720, output.Results.ElementAt(3).Item2);
        }

        #endregion


    }
}
