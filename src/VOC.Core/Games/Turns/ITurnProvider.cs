﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns
{
    public interface ITurnProvider
    {
        ITurn GetNext();
    }
}
