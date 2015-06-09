using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Model
{
    public class PlayerHistorical
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int StartRating { get; set; }
        public int EndRating { get; set; }
    }
}
