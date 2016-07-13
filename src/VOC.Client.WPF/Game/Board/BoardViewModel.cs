using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.WPF.Game.Board
{
    public class BoardViewModel
    {
        public ObservableCollection<TileViewModel> Tiles { get; } = new ObservableCollection<TileViewModel>();

        public BoardViewModel()
        {
            Tiles.Add(new TileViewModel(0, 0));
            Tiles.Add(new TileViewModel(1, 0));
            Tiles.Add(new TileViewModel(-1, 0));
            Tiles.Add(new TileViewModel(0, 1));
            Tiles.Add(new TileViewModel(0, -1));
        }
    }
}
