using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Configuration
{
    public class DummyMap : IMap
    {
        public DummyMap(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
