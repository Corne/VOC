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
                //ToDo CvB: Max inventory space? 

                materials.Add(rawMaterial);
            }
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
                for (int i=0; i<resources.Length; i++)
                {
                    var firstMatching = materials.First(m => m.Type == resources[i]);
                    materials.Remove(firstMatching);
                    result[i] = firstMatching;
                }
                return result;
            }
        }
    }
}
