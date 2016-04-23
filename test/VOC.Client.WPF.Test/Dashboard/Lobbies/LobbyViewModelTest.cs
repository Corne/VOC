using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Dashboard.Lobbies;
using VOC.Client.WPF.Dashboard.Lobbies;
using Xunit;

namespace VOC.Client.WPF.Test.Dashboard.Lobbies
{
    public class LobbyViewModelTest
    {


        [Fact]
        public void CantConstructWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new LobbyViewModel(null));
        }

        [Fact]
        public void PlayersWillContainLobbyPlayers()
        {
            var lobby = new Mock<ILobby>();
            IEnumerable<Player> players = new[] { new Player(Guid.NewGuid(), "Bob"), new Player(Guid.NewGuid(), "Henk"), new Player(Guid.NewGuid(), "Jan") };
            lobby.Setup(l => l.Players).Returns(players);

            var viewmodel = new LobbyViewModel(lobby.Object);

            Assert.Equal(players, viewmodel.Players);
        }

        [Fact]
        public void AddedPlayersToLobbyWillBeAddedToViewModel()
        {
            var lobby = new Mock<ILobby>();
            IEnumerable<Player> players = new[] { new Player(Guid.NewGuid(), "Bob") };
            lobby.Setup(l => l.Players).Returns(players);

            var viewmodel = new LobbyViewModel(lobby.Object);
            var input = new Player(Guid.NewGuid(), "Marley");
            lobby.Raise(l => l.PlayerJoined += null, lobby, input);

            Assert.Contains(input, viewmodel.Players);
        }

        [Fact]
        public void LeftPlayerFromLobbyWillBeRemovedFromViewModel()
        {
            var lobby = new Mock<ILobby>();
            var input = new Player(Guid.NewGuid(), "Marley");

            IEnumerable<Player> players = new[] { new Player(Guid.NewGuid(), "Bob"), input };
            lobby.Setup(l => l.Players).Returns(players);

            var viewmodel = new LobbyViewModel(lobby.Object);
            lobby.Raise(l => l.PlayerLeft += null, lobby, input);

            Assert.DoesNotContain(input, viewmodel.Players);
        }
    }
}
