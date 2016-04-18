using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Dashboard.Configuration;
using Xunit;

namespace VOC.Client.Test.Dashboard.Configuration
{
    public class GameConfigurationTest
    {
        [Fact]
        public void CantConstructWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new GameConfiguration(null, 4));
        }

        [Theory]
        [InlineData(3, 4, 2)]
        [InlineData(3, 4, 5)]
        [InlineData(2, 8, 1)]
        [InlineData(2, 8, 9)]
        public void PlayerTotalShouldBeBetweenMinAndMaxMapSetting(int min, int max, int input)
        {
            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(min);
            map.Setup(m => m.MaxPlayers).Returns(max);
            Assert.Throws<ArgumentException>(() => new GameConfiguration(map.Object, input));
        }
    }
}
