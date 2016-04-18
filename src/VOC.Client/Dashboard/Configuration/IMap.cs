﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public interface IMap
    {
        string Name { get; }

        int MinPlayers { get; }
        int MaxPlayers { get; }
    }
}
