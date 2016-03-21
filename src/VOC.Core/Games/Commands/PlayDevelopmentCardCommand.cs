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
        private readonly IGame game;
        private readonly IDevelopmentCard card;
        public PlayDevelopmentCardCommand(IPlayer player, IGame game, IDevelopmentCard card)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (game == null)
                throw new ArgumentNullException(nameof(game));
            if (card == null)
                throw new ArgumentNullException(nameof(card));

            Player = player;
            this.game = game;
            this.card = card;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.PlayDevelopmentCard; } }

        public void Execute()
        {
            if (!card.Playable)
                throw new InvalidOperationException("Card is not playable");

            game.PlayDevelopmentCard(card);
            card.Played = true;
        }
    }
}
