using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using VOC.Client.Dashboard.Configuration;
using VOC.Client.WPF.GameConfiguration;
using Xunit;

namespace VOC.Client.WPF.Test.GameConfiguration
{
    public class ConfigurationViewModelTest
    {
        [Fact]
        public void CantBeConstructedWithNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ConfigurationViewModel(null));
        }

        [Fact]
        public async Task MapsGetLoadedOnNavigate()
        {
            var selector = new Mock<IMapSelector>();
            selector.Setup(s => s.GetMaps()).Returns(Task.FromResult(new[] { new Mock<IMap>().Object, new Mock<IMap>().Object }.AsEnumerable()));
            var viewmodel = new ConfigurationViewModel(selector.Object);

            await viewmodel.OnNavigate();
            Assert.Equal(2, viewmodel.Maps.Count);
        }
    }
}
