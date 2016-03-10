using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;

namespace VOC.Core.Items.Cards
{
    public class DevelopmentCard : IDevelopmentCard
    {
        private bool _playable;
        private ITurn turn;
        public DevelopmentCard(DevelopmentCardType type, ITurn boughtTurn)
        {
            if (!Enum.IsDefined(typeof(DevelopmentCardType), type))
                throw new ArgumentException("Invalid card type");
            if (boughtTurn == null)
                throw new ArgumentNullException(nameof(boughtTurn));

            Type = type;

            if (type == DevelopmentCardType.VictoryPoint)
                _playable = true;
            else {
                turn = boughtTurn;
                turn.Ended += Turn_Ended;
            }
        }

        private void Turn_Ended(object sender, EventArgs e)
        {
            if (turn != null)
            {
                turn.Ended -= Turn_Ended;
                turn = null;
            }
            _playable = true;
        }

        public bool Playable { get { return _playable && !Played; } }
        public DevelopmentCardType Type { get; }
        public bool Played { get; set; }
    }
}
