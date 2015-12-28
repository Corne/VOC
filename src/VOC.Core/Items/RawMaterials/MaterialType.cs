using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items.RawMaterials
{
    public enum MaterialType
    {
        /// <summary>
        /// Unsourced / None
        /// Will be used for the desert / tiles that don't provide resources
        /// </summary>
        Unsourced,
        /// <summary>
        /// We use tiles to display the sea
        /// </summary>
        Sea,
        Brick,
        Lumber,
        Wool,
        Grain,
        Ore
    }
}
