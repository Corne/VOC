using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games
{
    public class GameContainerTest
    {
        [Fact]
        public void TestGameConstruction()
        {
            var players = new HashSet<IPlayer>() {
                new Mock<IPlayer>().Object,
                new Mock<IPlayer>().Object,
                new Mock<IPlayer>().Object,
                new Mock<IPlayer>().Object
            };
            using (var factory = new GameContainer())
            {
                var game = factory.Create(players);
                Assert.NotNull(game);
            }
        }


    }
}
