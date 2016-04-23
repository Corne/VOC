using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Dashboard.Games;
using VOC.Client.WPF.Dashboard.Overview;
using VOC.Client.WPF.Main.Navigation;
using Xunit;

namespace VOC.Client.WPF.Test.Dashboard.Overview
{
    public class DashboardViewModelTest
    {
        public static IEnumerable<object> NullConstruction
        {
            get
            {
                yield return new object[] { null, new Mock<INavigationService>().Object };
                yield return new object[] { new Mock<IGameStore>().Object, null };
            }
        }

        [Theory, MemberData(nameof(NullConstruction))]
        public void CantBeConstructedWithNull(IGameStore store, INavigationService navigation)
        {
            Assert.Throws<ArgumentNullException>(() => new DashboardViewModel(store, navigation));
        }

        [Fact]
        public async Task GameStoreGetsLoadedOnNavigate()
        {
            var store = new Mock<IGameStore>();
            var navigation = new Mock<INavigationService>();
            var viewmodel = new DashboardViewModel(store.Object, navigation.Object);

            await viewmodel.OnNavigate();

            store.Verify(s => s.Load());
        }

        [Fact]
        public async Task GameStoreCreatesGameVMForeachGameInStore()
        {
            var store = new Mock<IGameStore>();
            var navigation = new Mock<INavigationService>();

            var games = Enumerable.Range(0, 5).Select(i => new Mock<IGame>().Object).ToList();
            store.Setup(s => s.Games).Returns(games);
            var viewmodel = new DashboardViewModel(store.Object, navigation.Object);

            await viewmodel.OnNavigate();
            Assert.Equal(games.Count, viewmodel.Games.Count);
        }
    }
}
