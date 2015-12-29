using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Items
{
    public interface IDie
    {

        /// <summary>
        /// Throw the die
        /// </summary>
        /// <returns>result</returns>
        int Throw();
    }
}
