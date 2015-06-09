using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface IGameHistoryService
    {
        Task<bool> Save(Model.Game game);
    }
}
