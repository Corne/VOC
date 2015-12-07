using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    public interface ITile
    {
        int Number { get; }
        IRawMaterial Rawmaterial { get; }
    }
}
