using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Client.WPF.Game.Board
{
    public class TileViewModel
    {
        public int X { get; }
        public int Y { get; }

        public TileViewModel(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
