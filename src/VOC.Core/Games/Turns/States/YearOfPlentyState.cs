﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Games.Turns.States
{
    public class YearOfPlentyState : ITurnState
    {
        public IEnumerable<GameCommand> Commands
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void AfterExecute(GameCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
