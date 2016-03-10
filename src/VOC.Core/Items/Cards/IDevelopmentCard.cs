﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items.Cards
{
    public interface IDevelopmentCard
    {
        DevelopmentCardType Type { get; }
        bool Playable { get; }
        bool Played { get; set; }
    }
}
