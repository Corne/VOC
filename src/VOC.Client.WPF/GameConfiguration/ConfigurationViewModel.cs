using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using VOC.Client.Dashboard.Configuration;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.GameConfiguration
{
    public class ConfigurationViewModel : ViewModelBase, IContentViewModel
    {
        private readonly IMapSelector mapSelector;

        public ConfigurationViewModel(IMapSelector mapSelector)
        {
            if (mapSelector == null)
                throw new ArgumentNullException(nameof(mapSelector));
            this.mapSelector = mapSelector;
        }

        public int Port { get; set; } = 1337;
        public ObservableCollection<IMap> Maps { get; } = new ObservableCollection<IMap>();

        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public async Task OnNavigate()
        {
            var maps = await mapSelector.GetMaps();
            foreach (var map in maps)
                Maps.Add(map);
        }
    }
}
