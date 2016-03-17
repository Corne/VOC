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
    public class YearOfPlentyCommand : IPlayerCommand
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

        private readonly MaterialType material1;
        private readonly MaterialType material2;

        public YearOfPlentyCommand(IPlayer player, MaterialType material1, MaterialType material2)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (!validmaterials.Contains(material1))
                throw new ArgumentException($"Material1 ({material1}) is not valid");
            if (!validmaterials.Contains(material2))
                throw new ArgumentException($"Material2 ({material2}) is not valid");

            Player = player;
            this.material1 = material1;
            this.material2 = material2;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.YearOfPlenty; } }
        

        public void Execute()
        {
            MaterialType[] materials = { material1, material2 };
            foreach (var material in materials)
                Player.AddResources(new RawMaterial(material));
        }
    }
}
