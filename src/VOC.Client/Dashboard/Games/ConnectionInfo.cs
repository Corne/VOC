using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.Dashboard.Games
{
    public class ConnectionInfo
    {
        public ConnectionInfo(IPAddress address, int port)
        {
            IPAddress = address;
            Port = port;
        }

        public IPAddress IPAddress { get; }
        public int Port { get; }
    }
}
