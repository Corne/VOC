using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    public class Tile : ITile
    {

        public Tile(int x, int y, int number, MaterialType material)
        {
            X = x;
            Y = y;
            Number = number;
            Rawmaterial = material;
        }
        public int Number { get; }

        public MaterialType Rawmaterial { get; }

        public int X { get; }

        public int Y { get; }

        public IRawMaterial Farm()
        {
            return new RawMaterial(Rawmaterial);
        }
    }
}
