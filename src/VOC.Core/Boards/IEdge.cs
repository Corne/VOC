﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOC.Core.Boards
{
    public interface IEdge : IBoardComponent
    {
        /// <summary>
        /// X coordinate of the edge
        /// </summary>
        int X { get; }
        /// <summary>
        /// Y coordiante of the edge
        /// </summary>
        int Y { get; }
        /// <summary>
        /// Side of the edge based on north side of adjacent tile
        /// </summary>
        EdgeSide Side { get; }
    }

    //Side should always be based on the north side of the tile, so South should never be an option
    //see: http://www-cs-students.stanford.edu/~amitp/game-programming/grids/hexagon-grid-edge-coordinates.png
    public enum EdgeSide
    {
        North,
        East,
        West
    }
}
