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

        public GameController() 
        {
            var context = new Core.MongoContext("mongodb://192.168.2.140", "PlayFoos");
            _gameService = new GameService(context);
        }

        // POST: api/New
        public async Task<IHttpActionResult> Post()
        {
            try
            {
                if (await _gameService.NewAsync() == null)
                    return BadRequest("A game is already in progress!");
                else
                    return Ok();
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
