using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items.RawMaterials
{
    public class RawMaterial : IRawMaterial
    {
        public RawMaterial(MaterialType type)
        {
            Type = type;
        }

        public MaterialType Type { get; }
    }
}
