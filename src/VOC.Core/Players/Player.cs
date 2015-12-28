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

            //ToDo CvB: Max inventory space? 

            materials.Add(rawMaterial);
        }
    }
}
