using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Services
{
    public interface ITableHeartbeatService
    {
        Task<DateTime?> GetLatestAsync();

        Task UpdateAsync(DateTime timestamp, string error);
    }
}
