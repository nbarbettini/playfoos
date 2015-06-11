using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PlayFoos.Core.Services;
using PlayFoos.API.Controllers;
using System.Web.Http.Results;
using PlayFoos.API.Communication;

namespace PlayFoos.API.Tests.Controllers
{
    [TestClass]
    public class ScoreControllerTests
    {
        private Mock<IGameService> _scoreServiceMock;
        private ScoreController _controller;

        [TestInitialize]
        public void Setup()
        {
            _scoreServiceMock = new Mock<IGameService>(MockBehavior.Strict);
            _controller = new ScoreController(_scoreServiceMock.Object);
        }

        [TestMethod]
        [TestCategory("API.ScoreController")]
        public async Task When_updating_score_for_bad_team()
        {
            var result = await _controller.Put(2) as BadRequestErrorMessageResult;

            Assert.AreEqual("Team not found", result.Message);
        }

        [TestMethod]
        [TestCategory("API.ScoreController")]
        public async Task When_updating_score_and_exception_occurs()
        {
            // Mock will fail with behavior Strict

            var result = await _controller.Put(0) as ExceptionResult;

            Assert.IsInstanceOfType(result.Exception, typeof(MockException));
        }

        [TestMethod]
        [TestCategory("API.ScoreController")]
        public async Task When_updating_score_and_service_fails()
        {
            _scoreServiceMock.Setup(m => m.UpdateScoreAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            var result = await _controller.Put(1) as BadRequestErrorMessageResult;

            Assert.AreEqual("Could not update score", result.Message);
        }

        [TestMethod]
        [TestCategory("API.ScoreController")]
        public async Task When_adding_score()
        {
            _scoreServiceMock.Setup(m => m.UpdateScoreAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _controller.Put(1) as OkResult;

            _scoreServiceMock.Verify(m => m.UpdateScoreAsync(1, 1));
        }

        [TestMethod]
        [TestCategory("API.ScoreController")]
        public async Task When_decreasing_score()
        {
            _scoreServiceMock.Setup(m => m.UpdateScoreAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            var result = await _controller.Put(1, false) as OkResult;

            _scoreServiceMock.Verify(m => m.UpdateScoreAsync(1, -1));
        }
    }
}
