using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Turns;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class DiscardResourcesCommand : IPlayerCommand
    {
        private readonly IEnumerable<MaterialType> discards;
        public DiscardResourcesCommand(IPlayer player, IEnumerable<MaterialType> discards)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (discards == null)
                throw new ArgumentNullException(nameof(discards));
            double inventoryCount = player.Inventory.Count();
            if (inventoryCount < 7)
                throw new InvalidOperationException("This player should not have to remove resources");
            int desired = Convert.ToInt32(Math.Round(inventoryCount / 2, MidpointRounding.AwayFromZero));
            if (inventoryCount - desired != discards.Count())
                throw new ArgumentException("Number of discard resources should be half of the player resources (rounded down)");

            Player = player;
            this.discards = discards;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.DiscardResources; } }

        public void Execute()
        {
            Player.TakeResources(discards.ToArray());
        }
    }
}
