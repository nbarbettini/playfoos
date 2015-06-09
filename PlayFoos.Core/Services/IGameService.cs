using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface IGameService
    {
        Task<Model.Game> GetCurrentAsync();

        Task<bool> DeleteCurrentAsync();

        Task<bool> UpdateScoreAsync(int side, int amount);

        Task<Model.Game> NewAsync();
    }
}
