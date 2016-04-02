using System;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class BuyDevelopmentCardCommand : IPlayerCommand
    {
        private readonly IGame game;

        public BuyDevelopmentCardCommand(IPlayer player, IGame game)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (game == null)
                throw new ArgumentNullException(nameof(game));

            Player = player;
            this.game = game;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.BuyDevelopmentCard; } }

        public void Execute()
        {
            game.BuyDevelopmentCard(Player);
        }
    }
}
