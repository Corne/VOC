﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class MoveRobberState : ITurnState
    {
        private readonly ITurn turn;
        private readonly IRobber robber;

        public MoveRobberState(ITurn turn, IRobber robber)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (robber == null)
                throw new ArgumentNullException(nameof(robber));

            this.turn = turn;
            this.robber = robber;
        }

        public IEnumerable<StateCommand> Commands
        {
            get { return new StateCommand[] { StateCommand.MoveRobber }; }
        }

        public void Start()
        {
            Stop();
            robber.Moved += Robber_Moved;
        }

        private void Robber_Moved(object sender, Boards.ITile e)
        {
            Stop();
            turn.SetState<RobberStealState>();
        }

        public void Stop()
        {
            robber.Moved -= Robber_Moved;
        }
    }
}