using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Data.Games;
using VOC.Client.WPF.Dashboard;
using Xunit;

namespace VOC.Client.WPF.Test.Dashboard
{
    public class DashboardViewModelTest
    {
        [Fact]
        public void CantBeConstructedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new DashboardViewModel(null));
        }

        [Fact]
        public async Task GameStoreGetsLoadedOnNavigate()
        {
            var store = new Mock<IGameStore>();
            var viewmodel = new DashboardViewModel(store.Object);

            await viewmodel.OnNavigate();

            store.Verify(s => s.Load());
        }

        [Fact]
        public async Task GameStoreCreatesGameVMForeachGameInStore()
        {
            var store = new Mock<IGameStore>();
            var games = Enumerable.Range(0, 5).Select(i => new Mock<IGame>().Object).ToList();
            store.Setup(s => s.Games).Returns(games);
            var viewmodel = new DashboardViewModel(store.Object);

            await viewmodel.OnNavigate();
            Assert.Equal(games.Count, viewmodel.Games.Count);
        }
    }
}
