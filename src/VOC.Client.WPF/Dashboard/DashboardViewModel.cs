using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using VOC.Client.Dashboard.Games;
using VOC.Client.WPF.GameConfiguration;
using VOC.Client.WPF.Main;
using VOC.Client.WPF.Main.Navigation;

namespace VOC.Client.WPF.Dashboard
{
    public class DashboardViewModel : IContentViewModel
    {
        private readonly IGameStore gamestore;
        private readonly INavigationService navigation;

        public DashboardViewModel(IGameStore gamestore, INavigationService navigation)
        {
            if (gamestore == null)
                throw new ArgumentNullException(nameof(gamestore));
            if (navigation == null)
                throw new ArgumentNullException(nameof(navigation));
            this.gamestore = gamestore;
            this.navigation = navigation;
        }

        public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>();

        public ICommand NavigateToGameConfiguration
        {
            get { return new RelayCommand(() => navigation.Navigate<ConfigurationViewModel>()); }
        }

        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public async Task OnNavigate()
        {
            await gamestore.Load();
            foreach (var game in gamestore.Games)
                Games.Add(new GameViewModel(game));
        }

    }
}
