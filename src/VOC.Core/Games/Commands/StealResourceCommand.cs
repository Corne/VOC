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
    public class StealResourceCommand : IPlayerCommand
    {
        private static readonly Random random = new Random();
        private readonly IPlayer victim;

        public StealResourceCommand(IPlayer player, IPlayer victim)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (victim == null)
                throw new ArgumentNullException(nameof(victim));
            if (victim.Inventory.Count() == 0)
                throw new InvalidOperationException("Can't steal from a player without resources");

            Player = player;
            this.victim = victim;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.StealResource; } }

        public void Execute()
        {
            IRawMaterial[] inventory = victim.Inventory.ToArray();
            MaterialType type = inventory[random.Next(inventory.Length)].Type;
            var resource = victim.TakeResources(type);
            Player.AddResources(resource);
        }
    }
}
