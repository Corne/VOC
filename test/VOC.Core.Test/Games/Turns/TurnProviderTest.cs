using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Turns
{
    public class TurnProviderTest
    {
        [Fact]
        public void TurnProviderCantBeCreatedWithoutPlayers()
        {
            Assert.Throws<ArgumentNullException>(() => new TurnProvider(null));
        }

        //[Fact]
        //public void GetNextReturnsTurnWithHighRollProvider()
        //{
        //    var players = new HashSet<IPlayer>();
        //    players.Add(new Mock<IPlayer>().Object);
        //    var provider = new TurnProvider(players);

        //    var turn = provider.GetNext();
        //    Assert.IsType<HighRollTurn>(turn);
        //}
    }
}
