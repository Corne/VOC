using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public interface IMapConfigurator
    {
        Task<IEnumerable<IMap>> GetMaps();
    }
}
