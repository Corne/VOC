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

        [Fact]
        public void AddResourceTest()
        {
            var material = new Mock<IRawMaterial>();

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
    }
}
