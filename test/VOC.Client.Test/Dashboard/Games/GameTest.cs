using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Dashboard.Games;
using Xunit;

namespace VOC.Client.Test.Dashboard.Games
{
    public class GameTest
    {
        public static IEnumerable<object> NullConstruction {
            get
            {
                yield return new object[] { null, new ConnectionInfo(new IPAddress(new byte[] { 127, 0, 0, 1 }), 1337) };
                yield return new object[] { "Game 1", null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantBeConstructedWithNull(string name, ConnectionInfo connection)
        {
            Assert.Throws<NullReferenceException>(() => new Game(name, connection));
        }
    }
}
