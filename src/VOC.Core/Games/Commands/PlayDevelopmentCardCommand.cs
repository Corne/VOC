using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Cards;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class PlayDevelopmentCardCommand : IPlayerCommand
    {
        private readonly IGameTurn turn;
        private readonly IDevelopmentCard card;
        public PlayDevelopmentCardCommand(IPlayer player, IGameTurn turn, IDevelopmentCard card)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            Player = player;
            this.turn = turn;
            this.card = card;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.PlayDevelopmentCard; } }

        public void Execute()
        {
            if (!card.Playable)
                throw new InvalidOperationException("Card is not playable");

            turn.PlayDevelopmentCard(card);
            card.Played = true;
        }
    }
}
