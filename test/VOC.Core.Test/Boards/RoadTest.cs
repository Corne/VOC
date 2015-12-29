using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Boards
{
    public class RoadTest
    {
        [Fact]
        public void OwnerCantBeNull()
        {
            var edge = new Mock<IEdge>();
            Assert.Throws<ArgumentNullException>(() => new Road(edge.Object, null));
        }

        [Fact]
        public void EdgeCantBeNull()
        {
            var owner = new Mock<IPlayer>();
            Assert.Throws<ArgumentNullException>(() => new Road(null, owner.Object));
        }

        [Fact]
        public void ConstructionTest()
        {
            var edge = new Mock<IEdge>();
            var owner = new Mock<IPlayer>();
            var road = new Road(edge.Object, owner.Object);

            Assert.NotNull(road);
            Assert.Equal(edge.Object, road.Edge);
            Assert.Equal(owner.Object, road.Owner);
        }
    }
}
