using System.Threading.Tasks;
using Moq;
using VOC.Client.WPF.Main;
using Xunit;

namespace VOC.Client.WPF.Test.Main
{
    public class MainViewModelTest
    {

        [Fact]
        public async Task UpdateNavigatesToContent()
        {
            var content = new Mock<IContentViewModel>();
            var viewmodel = new MainViewModel();

            await viewmodel.Update(content.Object);

            content.Verify(c => c.OnNavigate());
        }

        [Fact]
        public async Task UpdateClosesOldContent()
        {
            var old = new Mock<IContentViewModel>();
            var current = new Mock<IContentViewModel>();

            var viewmodel = new MainViewModel();

            await viewmodel.Update(old.Object);
            await viewmodel.Update(current.Object);

            old.Verify(o => o.OnClose());
        }

        [Fact]
        public async Task UpdateRaisesChangeEvent()
        {
            var content = new Mock<IContentViewModel>();
            var viewmodel = new MainViewModel();

            bool called = false;
            viewmodel.PropertyChanged += (sender, args) =>
            {
                called = true;
                Assert.Equal(nameof(viewmodel.Content), args.PropertyName);
            };
            await viewmodel.Update(content.Object);

            Assert.True(called);
        }
    }
}
