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
        public ObservableCollection<TileViewModel> Tiles { get; }

        public BoardViewModel(IEnumerable<TileViewModel> tiles)
        {
            if (tiles == null || tiles.Any(t => t == null))
                throw new ArgumentNullException(nameof(tiles));
            Tiles = new ObservableCollection<TileViewModel>(tiles);

            TotalX = GetTotal(t => t.X);
            TotalY = GetTotal(t => t.Y);
        }

        private int GetTotal(Func<TileViewModel, int> property)
        {
            return Tiles.Select(property).DefaultIfEmpty(0).Max() + 1;
        } 

        public int TotalX { get; }
        public int TotalY { get; }
    }
}
