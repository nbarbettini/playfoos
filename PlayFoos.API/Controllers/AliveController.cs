using PlayFoos.API.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PlayFoos.API.Controllers
{
    public class AliveController : ApiController
    {
        private readonly IEngineChannel _engineChannel;

        public AliveController(IEngineChannel engineChannel)
        {
            _engineChannel = engineChannel;
        }

        // GET: api/alive
        public async Task<bool> Get()
        {
            await _engineChannel.StartIfNecessary();
            return _engineChannel.IsConnected();
        }
    }
}
