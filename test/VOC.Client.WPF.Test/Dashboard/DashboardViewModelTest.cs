using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Data.Store;
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
    }
}
