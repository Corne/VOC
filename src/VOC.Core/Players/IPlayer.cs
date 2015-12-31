using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Players
{
    /// <summary>
    /// Interface that describes a player of the game, which can be an user or a bot.
    /// </summary>
    public interface IPlayer
    {
        string Name { get; }

        IEnumerable<IRawMaterial> Inventory { get; }

        void AddResource(IRawMaterial rawMaterial);
        bool HasResources(params MaterialType[] rawmaterials);
        void RemoveResources(params MaterialType[] bUILD_RESOURCES);
    }
}
