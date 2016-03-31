using System;
using Moq;
using VOC.Core.Items.Achievements;
using VOC.Core.Players;
using Xunit;

namespace VOC.Core.Test.Items.Achievements
{
    public class BiggestArmyTest
    {

        [Fact]
        public void UpdateFailsOnNull()
        {
            var achievement = new BiggestArmy();
            Assert.Throws<ArgumentNullException>(() => achievement.Update(null));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void UpdateDoesNotSetAnOwnerIfLessThen3Knights(int knightCount)
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.ArmySize).Returns(knightCount);

            var achievement = new BiggestArmy();
            bool changed = false;
            achievement.OwnerChanged += (sender, args) => { changed = true; };
            achievement.Update(player.Object);

            Assert.False(changed);
            Assert.Null(achievement.Owner);
        }

        [Fact]
        public void UpdateSetPlayerIfArmySize3()
        {
            var player = new Mock<IPlayer>();
            player.Setup(p => p.ArmySize).Returns(3);

            var achievement = new BiggestArmy();
            bool changed = false;
            achievement.OwnerChanged += (sender, args) => { changed = true; };
            achievement.Update(player.Object);

            Assert.True(changed);
            Assert.Equal(player.Object, achievement.Owner);
        }

        [Fact]
        public void UpdateDoesNotReplaceOwnerIfArmySizeEqualToCurrentOwner()
        {
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.ArmySize).Returns(4);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.ArmySize).Returns(4);

            var achievement = new BiggestArmy();
            achievement.Update(player1.Object);
            achievement.Update(player2.Object);

            Assert.Equal(player1.Object, achievement.Owner);
        }

        [Fact]
        public void UpdateReplacesOwnerIfArmyBiggerThenOwner()
        {
            var player1 = new Mock<IPlayer>();
            player1.Setup(p => p.ArmySize).Returns(3);

            var player2 = new Mock<IPlayer>();
            player2.Setup(p => p.ArmySize).Returns(4);

            var achievement = new BiggestArmy();
            achievement.Update(player1.Object);
            achievement.Update(player2.Object);

            Assert.Equal(player2.Object, achievement.Owner);
        }
    }
}
