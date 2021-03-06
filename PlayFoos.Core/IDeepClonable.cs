﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayFoos.Core
{
    internal interface IDeepClonable<T>
    {
        // Indicates the object exposes an internal copy constructor

        T DeepClone();
    }
}
