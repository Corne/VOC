using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Intergration.GameFlow
{
    public class SetupPhaseTest
    {
        [Fact]
        public void TestSetupPhase()
        {
            var player1 = new Player("Henk");
            var player2 = new Player("Bob");
            var player3 = new Player("Sjaak");
            var player4 = new Player("Kees");
            var players = new HashSet<IPlayer>() { player1, player2, player3, player4 };

            using (var factory = new GameContainer())
            {
                var game = factory.Create(players);
                var commandFactory = factory.GetCommandFactory(game);

                Assert.Equal(players, game.Players);

                int turnstartCount = 0;
                game.TurnStarted += (sender, args) => turnstartCount++;

                game.Start();
                Assert.Equal(1, turnstartCount);

                var rolldice = commandFactory.NewHighRollCommand(player1);
                game.Execute(rolldice);

                Assert.Equal(2, turnstartCount);
            }
        }
    }
}
