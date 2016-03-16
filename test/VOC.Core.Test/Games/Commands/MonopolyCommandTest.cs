using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Games.Commands;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class MonopolyCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new IPlayer[] { new Mock<IPlayer>().Object } };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNullParameter(IPlayer player, IEnumerable<IPlayer> players)
        {
            Assert.Throws<ArgumentNullException>(() => new MonopolyCommand(player, players, MaterialType.Brick));
        }

        public static IEnumerable<object> InvalidMaterials
        {
            get
            {
                yield return new object[] { (MaterialType)(-1) };
                yield return new object[] { MaterialType.Sea };
                yield return new object[] { MaterialType.Unsourced };
            }
        }

        [Theory, MemberData(nameof(InvalidMaterials))]
        public void MustConstructWithValidMaterialType(MaterialType material)
        {
            var player = new Mock<IPlayer>();
            var victims = new IPlayer[] { new Mock<IPlayer>().Object };
            Assert.Throws<ArgumentException>(() => new MonopolyCommand(player.Object, victims, material));
        }

        [Fact]
        public void CantConstructWithEmptyPlayersArray()
        {
            var player = new Mock<IPlayer>();
            var victims = new IPlayer[0];
            Assert.Throws<ArgumentException>(() => new MonopolyCommand(player.Object, victims, MaterialType.Grain));
        }

        [Fact]
        public void ConstructionFailsIfPlayerIsVictim()
        {
            var player = new Mock<IPlayer>();
            var victims = new IPlayer[] { new Mock<IPlayer>().Object, player.Object };
            Assert.Throws<ArgumentException>(() => new MonopolyCommand(player.Object, victims, MaterialType.Wool));
        }

        private static IPlayer CreatePlayer(params MaterialType[] materials)
        {
            var player = new Player("Test");
            player.AddResources(materials.Select(m => new RawMaterial(m)).ToArray());
            return player;
        }


        [Fact]
        public void ExecuteTest()
        {
            var player = new Player("Bob");
            player.AddResources(new RawMaterial(MaterialType.Brick));
            var victim1 = CreatePlayer(MaterialType.Brick, MaterialType.Grain, MaterialType.Ore);
            var victim2 = CreatePlayer(MaterialType.Grain, MaterialType.Grain, MaterialType.Brick, MaterialType.Grain);
            var victims = new IPlayer[] { victim1, victim2 };
            var command = new MonopolyCommand(player, victims, MaterialType.Grain);

            command.Execute();

            Assert.Equal(4, player.Inventory.Count(r => r.Type == MaterialType.Grain));
            Assert.Equal(5, player.Inventory.Count());
            Assert.Equal(0, victim1.Inventory.Count(r => r.Type == MaterialType.Grain));
            Assert.Equal(2, victim1.Inventory.Count());
            Assert.Equal(0, victim2.Inventory.Count(r => r.Type == MaterialType.Grain));
            Assert.Equal(1, victim2.Inventory.Count());
        }
    }
}
