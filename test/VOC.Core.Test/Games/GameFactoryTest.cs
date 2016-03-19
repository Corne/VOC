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
    public class GameFactoryTest
    {
        [Fact]
        public void TestGameConstruction()
        {
            using (var factory = new GameFactory())
            {
                var players = new HashSet<IPlayer>() {
                    new Mock<IPlayer>().Object,
                    new Mock<IPlayer>().Object,
                    new Mock<IPlayer>().Object,
                    new Mock<IPlayer>().Object
                };
                var game = factory.Create(players);

                Assert.NotNull(game);

                int turnstartCount = 0;
                game.TurnStarted += (sender, args) => turnstartCount++;

                game.Start();
                Assert.Equal(1, turnstartCount);
            }
        }


    }
}
