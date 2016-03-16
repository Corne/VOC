using System;
using System.Collections.Generic;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Games.Commands;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Games.Commands
{
    public class BuildDevelopmentRoadCommandTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IBoard>().Object, new Mock<IEdge>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, null, new Mock<IEdge>().Object };
                yield return new object[] { new Mock<IPlayer>().Object, new Mock<IBoard>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantConstructWithNullParameter(IPlayer player, IBoard board, IEdge edge)
        {
            Assert.Throws<ArgumentNullException>(() => new BuildDevelopmentRoadCommand(player, board, edge));
        }

        [Fact]
        public void ExecuteTest()
        {
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            var edge = new Mock<IEdge>();

            var command = new BuildDevelopmentRoadCommand(player.Object, board.Object, edge.Object);
            command.Execute();

            board.Verify(b => b.BuildRoad(edge.Object, player.Object));
            player.Verify(p => p.TakeResources(It.IsAny<MaterialType[]>()), Times.Never);
        }
    }
}
