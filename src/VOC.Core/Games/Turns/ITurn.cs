using System;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public interface ITurn
    {
        event EventHandler Ended;
        IPlayer Player { get; }


        bool CanExecute(GameCommand command);
        void AfterExecute(GameCommand command);

    }
}
