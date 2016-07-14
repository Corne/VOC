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
            if (x < 0 || y < 0)
                throw new ArgumentException($"X({x}) and Y({y}, should be positive");

            X = x;
            Y = y;
        }
    }
}
