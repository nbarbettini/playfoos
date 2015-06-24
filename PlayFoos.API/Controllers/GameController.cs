using PlayFoos.API.Communication;
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
    public class GameController : ApiController
    {
        private readonly IGameService _gameService;
        private readonly IEngineChannel _engineChannel;

        public GameController(IGameService gameService, IEngineChannel engineChannel) 
        {
            _gameService = gameService;
            _engineChannel = engineChannel;
        }

        // POST: api/Game
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (await _gameService.NewAsync(force: true) == null)
                    return BadRequest("A game is already in progress!");
                else
                {
                    await _engineChannel.TriggerUpdate();
                    return Ok();
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        // PUT: api/Game/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Game/5
        public void Delete(int id)
        {
        }
    }
}
