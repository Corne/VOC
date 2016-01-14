using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Players
{
    public class Player : IPlayer
    {
        private readonly object removeResourceLock = new object();
        private readonly List<IRawMaterial> materials;

        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player should have a name");

            Name = name;
            materials = new List<IRawMaterial>();
        }

        public string Name { get; }
        public IEnumerable<IRawMaterial> Inventory { get { return materials.AsReadOnly(); } }


        public void AddResource(IRawMaterial rawMaterial)
        {
            if (rawMaterial == null)
                throw new ArgumentNullException(nameof(rawMaterial));

            if (rawMaterial.Type == MaterialType.Unsourced || rawMaterial.Type == MaterialType.Sea)
                throw new ArgumentException("Unsourced and Sea are invalid resources");
            //ToDo CvB: Max inventory space? 

            materials.Add(rawMaterial);
        }

        public bool HasResources(params MaterialType[] rawmaterials)
        {
            if (rawmaterials == null)
                return false;

            var distinctTypes = rawmaterials.Distinct();
            var materialTypes = materials.Select(m => m.Type);
            return distinctTypes.All(t => materialTypes.Where(m => m == t).Count() >= rawmaterials.Where(m => m == t).Count()); 
        }

        public IEnumerable<IRawMaterial> TakeResources(params MaterialType[] resources)
        {
            if (resources == null)
                throw new ArgumentNullException(nameof(resources));

            lock (removeResourceLock)
            {
                List<IRawMaterial> result = new List<IRawMaterial>();
                if (!HasResources(resources))
                    throw new InvalidOperationException("Player doesn't have those resources");

                foreach(var resource in resources)
                {
                    var firstMatching = materials.First(m => m.Type == resource);
                    materials.Remove(firstMatching);
                    result.Add(firstMatching);
                }
                return result;
            }
        }
    }
}
