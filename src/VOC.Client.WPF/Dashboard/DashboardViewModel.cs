using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.Data.Store;
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

        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public Task OnNavigate()
        {
            return gamestore.Load();
        }
    }
}
