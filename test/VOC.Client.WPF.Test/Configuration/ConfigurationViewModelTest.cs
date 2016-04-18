using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Dashboard.Configuration;
using VOC.Client.WPF.Configuration;
using Xunit;

namespace VOC.Client.WPF.Test.Configuration
{
    public class ConfigurationViewModelTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<IMapConfigurator>().Object };
                yield return new object[] { new Mock<IGameConfigurator>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantBeConstructedWithNull(IGameConfigurator gameconfig, IMapConfigurator mapconfig)
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationViewModel(gameconfig, mapconfig));
        }

        [Fact]
        public async Task MapsGetLoadedOnNavigate()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();
            var map1 = new Mock<IMap>();
            mapConfigurator.Setup(s => s.GetMaps()).Returns(Task.FromResult(new[] { map1.Object,  new Mock<IMap>().Object }.AsEnumerable()));
            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            await viewmodel.OnNavigate();

            Assert.Equal(2, viewmodel.Maps.Count);
            Assert.Equal(map1.Object, viewmodel.SelectedMap);
        }

        [Theory]
        [InlineData(3, 4, new int[] { 3, 4})]
        [InlineData(2, 6, new int[] { 2, 3, 4, 5, 6})]
        public void ExpectSelectablePlayersToBeListFromMinMaxOfSelectedMap(int min, int max, IEnumerable<int> expected)
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();

            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(min);
            map.Setup(m => m.MaxPlayers).Returns(max);

            viewmodel.SelectedMap = map.Object;

            Assert.Equal(expected, viewmodel.Players);
            Assert.Equal(min, viewmodel.SelectedPlayerCount);
        }

        [Fact]
        public void ExpectSelectablePlayersToBeEmptyListWhenSelectedMapNull()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();

            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            //first set some intial values
            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(3);
            map.Setup(m => m.MaxPlayers).Returns(4);

            viewmodel.SelectedMap = map.Object;
            viewmodel.SelectedMap = null;

            Assert.Equal(new int[0], viewmodel.Players);
        }

        [Fact]
        public void ExpectStartGameToNotExecuteIfMapNull()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();
            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            viewmodel.Port = 8008;
            viewmodel.SelectedMap = null;
            viewmodel.SelectedPlayerCount = 3;
            viewmodel.StartGameCommand.Execute(null);

            gameconfig.Verify(g => g.Start(It.IsAny<GameConfiguration>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ExpectStartToNotExecuteIfPlayersNotWithinMapRange()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();
            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(3);
            map.Setup(m => m.MaxPlayers).Returns(4);

            viewmodel.Port = 8008;
            viewmodel.SelectedMap = map.Object;
            viewmodel.SelectedPlayerCount = 2;
            viewmodel.StartGameCommand.Execute(null);

            gameconfig.Verify(g => g.Start(It.IsAny<GameConfiguration>(), It.IsAny<int>()), Times.Never);
        }

        //CvB Todo: maybe check more then positive int, maybe check if we can start something on this port??
        [Fact]
        public void ExpectStartToNotExecuteOnInvalidPort()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();
            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(3);
            map.Setup(m => m.MaxPlayers).Returns(4);

            viewmodel.Port = 0;
            viewmodel.SelectedMap = map.Object;
            viewmodel.SelectedPlayerCount = 3;
            viewmodel.StartGameCommand.Execute(null);

            gameconfig.Verify(g => g.Start(It.IsAny<GameConfiguration>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void StartGameExecuteTest()
        {
            var mapConfigurator = new Mock<IMapConfigurator>();
            var gameconfig = new Mock<IGameConfigurator>();
            var viewmodel = new ConfigurationViewModel(gameconfig.Object, mapConfigurator.Object);

            var map = new Mock<IMap>();
            map.Setup(m => m.MinPlayers).Returns(3);
            map.Setup(m => m.MaxPlayers).Returns(4);

            viewmodel.Port = 8008;
            viewmodel.SelectedMap = map.Object;
            viewmodel.SelectedPlayerCount = 3;

            viewmodel.StartGameCommand.Execute(null);

            gameconfig.Verify(g => g.Start(It.Is<GameConfiguration>(c => c.Map == map.Object && c.TotalPlayers == 3), 8008));
        }
    }
}
