using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.Cards;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Players
{
    /// <summary>
    /// Interface that describes a player of the game, which can be an user or a bot.
    /// </summary>
    public interface IPlayer
    {
        Guid Id { get; }
        string Name { get; }

        IEnumerable<IRawMaterial> Inventory { get; }
        IEnumerable<IDevelopmentCard> Cards { get; }
        int ArmySize { get; }

        void AddResources(params IRawMaterial[] rawMaterials);
        bool HasResources(params MaterialType[] rawmaterials);
        /// <summary>
        /// Take resources from the player
        /// </summary>
        /// <param name="resources">resources from a specific materialtype</param>
        /// <returns>an array of the same size as resources.length, with materials with all the resource types</returns>
        IRawMaterial[] TakeResources(params MaterialType[] resources);
        void AddCard(IDevelopmentCard developmentCard);
        IDevelopmentCard FindCard(Guid id);

    }
}
