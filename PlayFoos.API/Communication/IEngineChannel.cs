using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.API.Communication
{
    public interface IEngineChannel
    {
        Task TriggerUpdate();
        Task StartIfNecessary();
        Task Stop();
        bool IsConnected();
    }
}
