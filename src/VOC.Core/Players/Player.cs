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

        public void RemoveResources(params MaterialType[] resources)
        {
            throw new NotImplementedException();
        }
    }
}
