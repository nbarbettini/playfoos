using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class GameStateDto
    {
        public int Id { get; set; }
        public bool Started { get; set; }
        public DateTime StartTime { get; set; }

    }
}
