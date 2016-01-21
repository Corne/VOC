using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns.States;
using VOC.Core.Items;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public interface ITurn
    {
        event EventHandler<ITurnState> StateChanged;
        IPlayer Player { get; }
        bool DevelopmentCardPlayed { get; }
        void PlayDevelopmentCard(IDevelopmentCard card);

        void NextFlowState();
        void SetState<T>() where T : ITurnState;

        bool CanExecute(StateCommand command);

        void Start();
        void End();
    }
}
