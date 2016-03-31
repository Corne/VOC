using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Core.Boards;
using VOC.Core.Items.Achievements;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Items.Achievements
{
    public class LongestRoadTest
    {
        [Fact]
        public void CantConstructWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LongestRoad(null));
        }

        [Fact]
        public void CantUpdateWithNullPlayer()
        {
            var board = new Mock<IBoard>();
            var achievement = new LongestRoad(board.Object);

            Assert.Throws<ArgumentNullException>(() => achievement.Update(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(4)]
        public void OwnerStaysNullIfRoadNotLongerThen4(int roadcount)
        {
            var roads = Enumerable.Range(0, roadcount).Select(i => new Mock<IRoad>().Object);
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetLongestRoad(player.Object)).Returns(roads);

            var achievement = new LongestRoad(board.Object);
            bool changed = false;
            achievement.OwnerChanged += (sender, args) => { changed = true; };

            achievement.Update(player.Object);

            Assert.False(changed);
            Assert.Null(achievement.Owner);
        }

        [Fact]
        public void PlayerTakesAchievementIfMinimumOf5()
        {
            var roads = Enumerable.Range(0, 5).Select(i => new Mock<IRoad>().Object);
            var player = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetLongestRoad(player.Object)).Returns(roads);

            var achievement = new LongestRoad(board.Object);
            bool changed = false;
            achievement.OwnerChanged += (sender, args) => { changed = true; };

            achievement.Update(player.Object);

            Assert.True(changed);
            Assert.Equal(player.Object, achievement.Owner);
        }

        [Fact]
        public void PlayerDoesNotOvertakeAchievementIfRoadOfSameSize()
        {
            var roads1 = Enumerable.Range(0, 5).Select(i => new Mock<IRoad>().Object);
            var roads2 = Enumerable.Range(0, 5).Select(i => new Mock<IRoad>().Object);
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetLongestRoad(player1.Object)).Returns(roads1);
            board.Setup(b => b.GetLongestRoad(player2.Object)).Returns(roads2);

            var achievement = new LongestRoad(board.Object);
            achievement.Update(player1.Object);
            achievement.Update(player2.Object);
            Assert.Equal(player1.Object, achievement.Owner);
        }

        [Fact]
        public void PlayerOvertakesAchievementIfonger()
        {
            var roads1 = Enumerable.Range(0, 5).Select(i => new Mock<IRoad>().Object);
            var roads2 = Enumerable.Range(0, 6).Select(i => new Mock<IRoad>().Object);
            var player1 = new Mock<IPlayer>();
            var player2 = new Mock<IPlayer>();
            var board = new Mock<IBoard>();
            board.Setup(b => b.GetLongestRoad(player1.Object)).Returns(roads1);
            board.Setup(b => b.GetLongestRoad(player2.Object)).Returns(roads2);

            var achievement = new LongestRoad(board.Object);
            achievement.Update(player1.Object);
            achievement.Update(player2.Object);
            Assert.Equal(player2.Object, achievement.Owner);
        }
    }
}
