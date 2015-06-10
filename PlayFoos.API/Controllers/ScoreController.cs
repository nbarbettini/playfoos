using PlayFoos.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PlayFoos.API.Controllers
{
    public class ScoreController : ApiController
    {
        private readonly IGameService _gameService;

        public ScoreController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // PUT: api/Score/1
        public async Task<IHttpActionResult> Put(int id, bool increase = true)
        {
            if (id < 0 || id > 1)
                return BadRequest("Team not found");

            int amount = increase ? 1 : -1;

            try
            {
                var good = await _gameService.UpdateScoreAsync(id, amount);

                if (good)
                    return Ok();
                else
                    return BadRequest("Could not update score");
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
