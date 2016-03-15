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
    public class StealResourceCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IPlayer>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNull(IPlayer player, IPlayer victim)
        {
            Assert.Throws<ArgumentNullException>(() => new StealResourceCommand(player, victim));
        }

        [Fact]
        public void ConstructionFailsIfVictimNoResources()
        {
            var player = new Mock<IPlayer>();
            var victim = new Mock<IPlayer>();
            victim.Setup(v => v.Inventory).Returns(new IRawMaterial[0]);
            Assert.Throws<InvalidOperationException>(() => new StealResourceCommand(player.Object, victim.Object));
        }

        private IRawMaterial CreateMaterial(MaterialType type)
        {
            var mock = new Mock<IRawMaterial>();
            mock.Setup(m => m.Type).Returns(type);
            return mock.Object;
        }

        [Fact]
        public void ExecuteTakesResourceFromVictimAndAddToPlayer()
        {
            var player = new Mock<IPlayer>();
            var victim = new Mock<IPlayer>();
            victim.Setup(v => v.Inventory).Returns(
                new IRawMaterial[2] {
                    CreateMaterial(MaterialType.Brick),
                    CreateMaterial(MaterialType.Lumber)
                });
            victim.Setup(v => v.TakeResources(It.IsAny<MaterialType[]>()))
                .Returns<MaterialType[]>(m => victim.Object.Inventory.Where(i => m.Contains(i.Type)).ToArray());

            var command = new StealResourceCommand(player.Object, victim.Object);
            command.Execute();

            victim.Verify(v => v.TakeResources(It.Is<MaterialType[]>(m => m.Length == 1)));
            player.Verify(v => v.AddResources(It.Is<IRawMaterial[]>(m => m.Length == 1)));
        }
    }
}
