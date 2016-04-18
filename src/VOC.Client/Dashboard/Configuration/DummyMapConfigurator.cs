using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public class DummyMapConfigurator : IMapConfigurator
    {
        public async Task<IEnumerable<IMap>> GetMaps()
        {
            await Task.FromResult(0);
            return new IMap[] { new DummyMap("Default") };
        }
    }
}
