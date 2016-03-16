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
    public class MonopolyCommand : IPlayerCommand
    {
        private static IEnumerable<MaterialType> validmaterials
        {
            get
            {
                return Enum.GetValues(typeof(MaterialType))
                    .Cast<MaterialType>()
                    .Except(new[] { MaterialType.Unsourced, MaterialType.Sea });
            }
        }
        private readonly IEnumerable<IPlayer> victims;
        private readonly MaterialType material;
        public MonopolyCommand(IPlayer player, IEnumerable<IPlayer> victims, MaterialType material)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (victims == null)
                throw new ArgumentNullException(nameof(victims));
            if (!victims.Any())
                throw new ArgumentException("Something went wrong with construction, expected all players from the game");
            if (victims.Contains(player))
                throw new ArgumentException("Player cant be a victim");
            if (!validmaterials.Contains(material))
                throw new ArgumentException($"This material is not valid {material}");

            Player = player;
            this.victims = victims;
            this.material = material;
        }

        public IPlayer Player { get; }
        public GameCommand Type { get { return GameCommand.Monopoly; } }

        public void Execute()
        {
            foreach(var victim in victims)
            {
                while (victim.HasResources(material))
                {
                    var resource = victim.TakeResources(material);
                    Player.AddResources(resource);
                }
            }
        }
    }
}
