using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Moq;
using VOC.Client.WPF.Main;
using VOC.Client.WPF.Main.Navigation;
using Xunit;

namespace VOC.Client.WPF.Test.Main.Navigation
{
    public class NavigationServiceTest : IDisposable
    {
        private enum Keys
        {
            Value1,
            Value2
        }

        private class DummyClass : IContentViewModel
        {
            public Task OnNavigate()
            {
                return Task.FromResult(0);
            }

            public Task OnClose()
            {
                return Task.FromResult(0);
            }


        }

        private class TestClass : IContentViewModel
        {
            public TestClass()
            {

            }

            public TestClass(string paramater)
            {
                Parameter = paramater;
            }

            public string Parameter { get; }

            public Task OnNavigate()
            {
                return Task.FromResult(0);
            }

            public Task OnClose()
            {
                return Task.FromResult(0);
            }

        }

        private MainViewModel main;
        private IContainer container;


        public NavigationServiceTest()
        {

            var builder = new ContainerBuilder();
            builder.RegisterInstance(new MainViewModel());

            builder.RegisterType<NavigationService>().AsSelf().As<INavigationService>();

            builder.RegisterType<DummyClass>().AsSelf().Keyed<IContentViewModel>(Keys.Value1);
            builder.RegisterType<TestClass>().AsSelf().Keyed<IContentViewModel>(Keys.Value2);

            builder.RegisterType<ContentScope>();

            container = builder.Build();

            main = container.Resolve<MainViewModel>();
        }


        public void Dispose()
        {
            container.Dispose();
        }

        [Fact]
        public async Task NavigateUpdatesMainContent()
        {
            var input = new Mock<IContentViewModel>();

            using (var service = container.Resolve<NavigationService>())
            {
                await service.Navigate<TestClass>();

                Assert.IsType<TestClass>(main.Content);
            }
        }

        [Fact]
        public async Task BackSetsPreviousViewModel()
        {
            var dummy = new Mock<IContentViewModel>();
            using (var service = container.Resolve<NavigationService>())
            {
                await service.Navigate<DummyClass>();
                await service.Navigate<TestClass>();
                await service.Back();

                Assert.IsType<DummyClass>(main.Content);
            }
        }

        [Fact]
        public async Task BackWontActOnHome()
        {
            using (var service = container.Resolve<NavigationService>())
            {
                await service.Navigate<DummyClass>();
                await service.Back();
                Assert.IsType<DummyClass>(main.Content);
            }
        }

        [Fact]
        public async Task HomeWillSetContentToHomeViewModel()
        {
            using (var service = container.Resolve<NavigationService>())
            {
                await service.Navigate<DummyClass>();
                await service.Navigate<TestClass>();
                await service.Navigate<TestClass>();
                await service.Navigate<TestClass>();

                await service.Home();
                Assert.IsType<DummyClass>(main.Content);
            }
        }

        [Fact]
        public async Task NavigateKeyedResolvesBasedOnRegisteredKey()
        {
            using (var service = container.Resolve<NavigationService>())
            {
                await service.NavigateKeyed<IContentViewModel>(Keys.Value2);
                Assert.IsType<TestClass>(main.Content);
            }
        }

        [Fact]
        public async Task ParametersWillBeInjectedOnNavigate()
        {
            using (var service = container.Resolve<NavigationService>())
            {
                string input = "Hello Parameter";
                var result = await service.Navigate<TestClass>(new TypedParameter(typeof(string), input));

                Assert.Equal(input, result.Parameter);
            }
        }
    }
}
