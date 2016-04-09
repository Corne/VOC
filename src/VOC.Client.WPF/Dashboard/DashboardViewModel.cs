using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Data.Games;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Dashboard
{
    public class DashboardViewModel : IContentViewModel
    {
        private readonly IGameStore gamestore;

        public DashboardViewModel(IGameStore gamestore)
        {
            if (gamestore == null)
                throw new ArgumentNullException(nameof(gamestore));

            this.gamestore = gamestore;
        }

        public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>();

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
