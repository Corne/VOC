using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public interface IMapSelector
    {
        Task<IEnumerable<IMap>> GetMaps();
    }
}
