using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using VOC.Core.Items.Cards;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Players
{
    public class Player : IPlayer
    {
        private static ILog logger = LogManager.GetLogger(nameof(Player));

        private readonly object removeResourceLock = new object();
        private readonly ISet<IRawMaterial> materials;
        private readonly ISet<IDevelopmentCard> cards;

        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player should have a name");

            Name = name;
            materials = new HashSet<IRawMaterial>();
            cards = new HashSet<IDevelopmentCard>();
        }

        public string Name { get; }
        public IEnumerable<IRawMaterial> Inventory { get { return materials.ToList().AsReadOnly(); } }
        public IEnumerable<IDevelopmentCard> Cards { get { return cards.ToList().AsReadOnly(); } }

        public void AddResources(params IRawMaterial[] rawMaterials)
        {
            if (rawMaterials == null)
                throw new ArgumentNullException(nameof(rawMaterials));

            foreach (var rawMaterial in rawMaterials)
            {
                if (rawMaterial == null)
                    throw new ArgumentNullException(nameof(rawMaterial));

                if (rawMaterial.Type == MaterialType.Unsourced || rawMaterial.Type == MaterialType.Sea)
                    throw new ArgumentException("Unsourced and Sea are invalid resources");

                materials.Add(rawMaterial);
            }

            logger.Info($"Added Rawmaterials to inventory {string.Join(", ", rawMaterials.Select(r => r.Type))}");
        }

        public bool HasResources(params MaterialType[] rawmaterials)
        {
            if (rawmaterials == null)
                return false;

            var distinctTypes = rawmaterials.Distinct();
            var materialTypes = materials.Select(m => m.Type);
            return distinctTypes.All(t => materialTypes.Where(m => m == t).Count() >= rawmaterials.Where(m => m == t).Count());
        }

        public IRawMaterial[] TakeResources(params MaterialType[] resources)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            lock (removeResourceLock)
            {
                if (!HasResources(resources))
                    throw new InvalidOperationException("Player doesn't have those resources");
                IRawMaterial[] result = new IRawMaterial[resources.Length];
                for (int i = 0; i < resources.Length; i++)
                {
                    var firstMatching = materials.First(m => m.Type == resources[i]);
                    materials.Remove(firstMatching);
                    result[i] = firstMatching;
                }

                logger.Info($"Removed resourced from player: {string.Join(", ", resources)}");

                return result;
            }
        }

        public void AddCard(IDevelopmentCard developmentCard)
        {
            if (developmentCard == null)
                throw new ArgumentNullException(nameof(developmentCard));
            cards.Add(developmentCard);
        }

        public IDevelopmentCard FindCard(Guid id)
        {
            return Cards.FirstOrDefault(c => c.Id == id);
        }
    }
}
