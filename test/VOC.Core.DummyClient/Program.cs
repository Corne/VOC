using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace VOC.Core.DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hubConnection = new HubConnection("http://localhost:8085"))
            {
                IHubProxy hubProxy = hubConnection.CreateHubProxy("GameHub");
                hubConnection.Start().Wait();

                hubProxy.Invoke("RollDice");
                Console.ReadLine();
            }
        }
    }
}
