using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Establishments
{
    /// <summary>
    /// Maybe changing to abstract class, in the future depending on our needs and EstablismentLevel
    /// </summary>
    public class Establishment : IEstablishment
    {
        public static readonly MaterialType[] BUILD_RESOURCES = { MaterialType.Lumber, MaterialType.Brick, MaterialType.Grain, MaterialType.Wool };
        public static readonly MaterialType[] UPGRADE_RESOURCES = { MaterialType.Grain, MaterialType.Grain, MaterialType.Ore, MaterialType.Ore, MaterialType.Ore };

        public Establishment(IPlayer owner, IVertex vertex)
        {
            Owner = owner;
            Vertex = vertex;
            //For now we manage the level and upgrading within the establisment
            //maybe later it will be cleaner to have seperate establisment classes
            Level = EstablishmentLevel.Settlement;
        }

        public IPlayer Owner { get; }
        public IVertex Vertex { get; }
        public EstablishmentLevel Level { get; private set; }

        public void Harvest(ITile tile)
        {
            if (!Vertex.IsAdjacentTo(tile))
                throw new ArgumentException("Can only harvest adjacent tiles!");

            for (int i = 0; i < (int)Level; i++)
            {
                IRawMaterial material = tile.Farm();
                Owner.AddResource(material);
            }
        }

        public void Upgrade()
        {
            if (Level == EstablishmentLevel.Settlement)
                Level = EstablishmentLevel.City;
            else
                throw new InvalidOperationException($"Can only upgrade from a settlement to a city, current level is {Level}");
        }
    }
}
