using System;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Users;
using VOC.Client.WPF.Main;
using VOC.Client.WPF.Main.Users;
using Xunit;

namespace VOC.Client.WPF.Test.Main
{
    public class MainViewModelTest
    {

        [Fact]
        public void CantBeConstructedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new MainViewModel(null));
        }

        private UserViewModel user = new UserViewModel(new Mock<IUser>().Object);

        [Fact]
        public async Task UpdateNavigatesToContent()
        {

            var content = new Mock<IContentViewModel>();
            var viewmodel = new MainViewModel(user);

            await viewmodel.Update(content.Object);

            content.Verify(c => c.OnNavigate());
        }

        [Fact]
        public async Task UpdateClosesOldContent()
        {
            var old = new Mock<IContentViewModel>();
            var current = new Mock<IContentViewModel>();

            var viewmodel = new MainViewModel(user);

            await viewmodel.Update(old.Object);
            await viewmodel.Update(current.Object);

            old.Verify(o => o.OnClose());
        }

        [Fact]
        public async Task UpdateRaisesChangeEvent()
        {
            var content = new Mock<IContentViewModel>();
            var viewmodel = new MainViewModel(user);

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
