using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core.Objects
{
    public sealed class FakeClock : IClock
    {
        private readonly Func<DateTime> _fakeNow;

        public FakeClock(Func<DateTime> fakeNow)
        {
            _fakeNow = fakeNow;
        }

        DateTime IClock.Now { get { return _fakeNow(); } }
    }
}
