using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Client.WPF.Game.Board;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Game
{
    public class GameViewModel : IContentViewModel
    {
        public BoardViewModel Board { get; }

        public GameViewModel()
        {
            TileViewModel[] tiles = {
                new TileViewModel(0, 0),
                new TileViewModel(1, 0),
                new TileViewModel(2, 0),
                new TileViewModel(0, 1),
                new TileViewModel(1, 1),
                new TileViewModel(2, 1),
                new TileViewModel(0, 2),
                new TileViewModel(1, 2),
                new TileViewModel(2, 2)
            };
            Board = new BoardViewModel(tiles);
        }


        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public Task OnNavigate()
        {
            return Task.FromResult(0);
        }

    }
}
