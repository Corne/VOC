using System;
using System.Collections.Generic;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games
{
    public interface IGame
    {
        /// <summary>
        /// Event that triggers when the game is finished, 
        /// will send the player that won as argument
        /// </summary>
       // event EventHandler<IPlayer> Finished;
        /// <summary>
        /// Event that will trigger when a new turn starts
        /// </summary>
        event EventHandler<ITurn> TurnStarted;

        IEnumerable<IPlayer> Players { get; }


        /// <summary>
        /// Start the game
        /// </summary>
        void Start();

        void Execute(IPlayerCommand command);
    }
}
