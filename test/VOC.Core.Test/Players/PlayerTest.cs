using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Players
{
    public class PlayerTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void PlayerShouldHaveAName(string name)
        {
            Assert.Throws<ArgumentException>(() => new Player(name));
        }

        [Theory]
        [InlineData(MaterialType.Brick)]
        [InlineData(MaterialType.Grain)]
        [InlineData(MaterialType.Lumber)]
        [InlineData(MaterialType.Ore)]
        [InlineData(MaterialType.Wool)]
        public void AddResourceTest(MaterialType type)
        {
            var material = new Mock<IRawMaterial>();
            material.Setup(m => m.Type).Returns(type);

            var player = new Player("Henk");
            player.AddResource(material.Object);

            Assert.Contains(material.Object, player.Inventory);
        }

        [Fact]
        public void CantAddNullResource()
        {
            var player = new Player("ABC");
            Assert.Throws<ArgumentNullException>(() => player.AddResource(null));
        }

        [Theory]
        [InlineData(MaterialType.Unsourced)]
        [InlineData(MaterialType.Sea)]
        public void CantAddUnsourcedOrSea(MaterialType type)
        {
            var material = new Mock<IRawMaterial>();
            material.Setup(m => m.Type).Returns(type);
            var player = new Player("Henk");

            Assert.Throws<ArgumentException>(() => player.AddResource(material.Object));
        }

        [Fact]
        public void HasResourceFalseOnNull()
        {
            var player = new Player("Bob");
            bool result = player.HasResources(null);

            Assert.False(result);
        }

        [Fact]
        public void HasResourcesTrueOnEmpty()
        {
            var player = new Player("Bob");
            bool result = player.HasResources(new MaterialType[] { });

            Assert.True(result);
        }


        [Theory]
        [InlineData(new MaterialType[] { MaterialType.Brick }, new MaterialType[] { MaterialType.Brick }, true)]
        [InlineData(new MaterialType[] { MaterialType.Lumber }, new MaterialType[] { MaterialType.Ore }, false)]
        [InlineData(new MaterialType[] { MaterialType.Grain }, new MaterialType[] { MaterialType.Grain, MaterialType.Grain }, false)]
        [InlineData(new MaterialType[] { MaterialType.Grain, MaterialType.Ore, MaterialType.Lumber }, new MaterialType[] { MaterialType.Ore, MaterialType.Lumber }, true)]
        [InlineData(new MaterialType[] { MaterialType.Grain, MaterialType.Ore, MaterialType.Ore }, new MaterialType[] { MaterialType.Ore, MaterialType.Ore }, true)]
        [InlineData(new MaterialType[] { MaterialType.Grain, MaterialType.Ore, MaterialType.Ore }, new MaterialType[] { MaterialType.Ore, MaterialType.Grain, MaterialType.Wool }, false)]
        public void HasResourcesTest(MaterialType[] current, MaterialType[] requested, bool expected)
        {
            var player = new Player("Bob");
            foreach (var material in current)
            {
                var mock = new Mock<IRawMaterial>();
                mock.Setup(m => m.Type).Returns(material);
                player.AddResource(mock.Object);
            }
            bool result = player.HasResources(requested);

            Assert.True(expected == result, $"Expected: {expected} != result {result}.\nCurrent:{string.Join(", ", current)}\nRequested:{string.Join(", ", requested)}");
        }

        [Fact]
        public void RemoveResourcesNullException()
        {
            var player = new Player("Bob");
            Assert.Throws<ArgumentNullException>(() => player.TakeResources(null));
        }

        [Theory]
        [InlineData(new MaterialType[] { }, new MaterialType[] { MaterialType.Brick })]
        [InlineData(new MaterialType[] { MaterialType.Grain }, new MaterialType[] { MaterialType.Wool })]
        [InlineData(new MaterialType[] { MaterialType.Ore }, new MaterialType[] { MaterialType.Ore, MaterialType.Ore })]
        [InlineData(new MaterialType[] { MaterialType.Ore, MaterialType.Wool, MaterialType.Lumber }, new MaterialType[] { MaterialType.Ore, MaterialType.Wool, MaterialType.Brick })]
        public void RemoveResourcesNotInInventoryException(MaterialType[] playerResources, MaterialType[] removeResources)
        {
            var player = new Player("Henk");

            foreach(var resource in playerResources)
            {
                var mock = new Mock<IRawMaterial>();
                mock.Setup(m => m.Type).Returns(resource);
                player.AddResource(mock.Object);
            }

            Assert.Throws<InvalidOperationException>(() => player.TakeResources(removeResources));
            Assert.True(player.HasResources(playerResources)); //assert nothing gets removed when failing
        }

        [Theory]
        [InlineData(
            new MaterialType[] { }, 
            new MaterialType[] { }, 
            new MaterialType[] { })]
        [InlineData(
            new MaterialType[] { MaterialType.Brick }, 
            new MaterialType[] { }, 
            new MaterialType[] { MaterialType.Brick })]
        [InlineData(
            new MaterialType[] { MaterialType.Brick }, 
            new MaterialType[] { MaterialType.Brick }, 
            new MaterialType[] { })]

        [InlineData(
            new MaterialType[] { MaterialType.Wool, MaterialType.Ore }, 
            new MaterialType[] { MaterialType.Wool, MaterialType.Ore }, 
            new MaterialType[] { })]

        [InlineData(
            new MaterialType[] { MaterialType.Grain, MaterialType.Grain }, 
            new MaterialType[] { MaterialType.Grain }, 
            new MaterialType[] { MaterialType.Grain })]

        [InlineData(
            new MaterialType[] { MaterialType.Grain, MaterialType.Grain }, 
            new MaterialType[] { MaterialType.Grain }, 
            new MaterialType[] { MaterialType.Grain })]

        [InlineData(
            new MaterialType[] { MaterialType.Grain, MaterialType.Grain, MaterialType.Lumber, MaterialType.Wool },
            new MaterialType[] { MaterialType.Grain, MaterialType.Lumber },
            new MaterialType[] { MaterialType.Grain, MaterialType.Wool })]

        [InlineData(
            new MaterialType[] { MaterialType.Ore, MaterialType.Grain, MaterialType.Lumber, MaterialType.Wool },
            new MaterialType[] { MaterialType.Ore, MaterialType.Grain, MaterialType.Lumber, MaterialType.Wool },
            new MaterialType[] {  })]

        [InlineData(
            new MaterialType[] { MaterialType.Ore, MaterialType.Ore, MaterialType.Ore, MaterialType.Lumber, MaterialType.Wool },
            new MaterialType[] { MaterialType.Ore, MaterialType.Ore },
            new MaterialType[] { MaterialType.Ore, MaterialType.Lumber, MaterialType.Wool })]
        public void RemoveResourcesTest(MaterialType[] playerResources, MaterialType[] removeResources, MaterialType[] expected)
        {
            var player = new Player("Henk");

            foreach (var resource in playerResources)
            {
                var mock = new Mock<IRawMaterial>();
                mock.Setup(m => m.Type).Returns(resource);
                player.AddResource(mock.Object);
            }

            IEnumerable<IRawMaterial> materials = player.TakeResources(removeResources);

            Assert.Equal(expected, player.Inventory.Select(i => i.Type));
            Assert.Equal(removeResources, materials.Select(m => m.Type));
            Assert.Equal(removeResources.Length, materials.Count());
        }
    }


}
