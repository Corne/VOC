using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.Cards;

namespace VOC.Core.Games.Turns.States
{
    public interface IStateProvider
    {
        ITurnState Get<T>() where T : ITurnState;
        ITurnState Get(DevelopmentCardType card);

        bool HasNext();
        ITurnState GetNext();
    }
}
