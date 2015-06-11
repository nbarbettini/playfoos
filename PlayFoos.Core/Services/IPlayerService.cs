using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface IPlayerService
    {
        Task<Model.Player> GetAsync(string email);

        Task<bool> NewAsync(string email, string password, string name);

        Task<bool> UpdateAsync(string email, bool won, int ratingAdjustment);

        Task<bool> UpdateAsync(string email, string password, string name);
    }
}
