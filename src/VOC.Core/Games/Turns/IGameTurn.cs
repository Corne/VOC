using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;

namespace VOC.Core.Games.Turns
{
    public interface IGameTurn : ITurn
    {
        event EventHandler<ITurnState> StateChanged;

        bool DevelopmentCardPlayed { get; }
        void PlayDevelopmentCard(IDevelopmentCard card);

        void NextFlowState();
        void SetState<T>() where T : ITurnState;


    }
}
