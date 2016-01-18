using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Items.RawMaterials;

namespace VOC.Core.Boards
{
    /// <summary>
    /// Creates the default board as shown in http://www.catan.com/en/download/?SoC_rv_Rules_091907.pdf
    /// For better visualisation / overlap with our grid documentation, we rotate the board so the robber is on top
    /// </summary>
    public class DefaultBoardBuilder : IBoardBuilder
    {
        public IEnumerable<IEdge> Edges { get; private set; }

        public IEnumerable<ITile> Tiles { get; private set; }

        public IEnumerable<IVertex> Vertices { get; private set; }

        public IEnumerable<IHarbor> Harbors { get; private set; }
        public void Build()
        {
            //we take 11,grain as center 0,0 and build from there see rotated image
            var tiles = new List<ITile>();
            var edges = new List<IEdge>();
            var vertices = new List<IVertex>();
            var harbors = new List<IHarbor>();

            tiles.Add(new Tile(0, 0, 11, MaterialType.Grain));
            tiles.Add(new Tile(0, 1, 3, MaterialType.Lumber));
            tiles.Add(new Tile(0, 2, 7, MaterialType.Unsourced));
            tiles.Add(new Tile(0, 3, -1, MaterialType.Sea)); //CvB Todo custom tile class cleaner for sea tiles, which dont have a number and a material?
            tiles.Add(new Tile(0, -1, 4, MaterialType.Lumber));
            tiles.Add(new Tile(0, -2, 8, MaterialType.Grain));
            tiles.Add(new Tile(0, -3, -1, MaterialType.Sea));

            tiles.Add(new Tile(-1, 0, 9, MaterialType.Wool));
            tiles.Add(new Tile(-1, 1, 10, MaterialType.Wool));
            tiles.Add(new Tile(-1, 2, 8, MaterialType.Brick));
            tiles.Add(new Tile(-1, 3, -1, MaterialType.Sea));
            tiles.Add(new Tile(-1, -1, 3, MaterialType.Ore));
            tiles.Add(new Tile(-1, -2, -1, MaterialType.Sea));

            tiles.Add(new Tile(-2, 0, 6, MaterialType.Lumber));
            tiles.Add(new Tile(-2, 1, 2, MaterialType.Grain));
            tiles.Add(new Tile(-2, 2, 5, MaterialType.Ore));
            tiles.Add(new Tile(-2, 3, -1, MaterialType.Sea));
            tiles.Add(new Tile(-2, -1, -1, MaterialType.Sea));

            //surrounding sea tiles
            tiles.Add(new Tile(-3, 0, -1, MaterialType.Sea));
            tiles.Add(new Tile(-3, 1, -1, MaterialType.Sea));
            tiles.Add(new Tile(-3, 2, -1, MaterialType.Sea));
            tiles.Add(new Tile(-3, 3, -1, MaterialType.Sea));

            tiles.Add(new Tile(1, 0, 6, MaterialType.Ore));
            tiles.Add(new Tile(1, 1, 4, MaterialType.Brick));
            tiles.Add(new Tile(1, 2, -1, MaterialType.Sea));
            tiles.Add(new Tile(1, -1, 5, MaterialType.Brick));
            tiles.Add(new Tile(1, -2, 10, MaterialType.Wool));
            tiles.Add(new Tile(1, -3, -1, MaterialType.Sea));

            tiles.Add(new Tile(2, 0, 11, MaterialType.Lumber));
            tiles.Add(new Tile(2, 1, -1, MaterialType.Sea));
            tiles.Add(new Tile(2, -1, 12, MaterialType.Wool));
            tiles.Add(new Tile(2, -2, 9, MaterialType.Grain));
            tiles.Add(new Tile(2, -3, -1, MaterialType.Sea));

            //surrounding sea tiles
            tiles.Add(new Tile(3, 0, -1, MaterialType.Sea));
            tiles.Add(new Tile(3, -1, -1, MaterialType.Sea));
            tiles.Add(new Tile(3, -2, -1, MaterialType.Sea));
            tiles.Add(new Tile(3, -3, -1, MaterialType.Sea));

            foreach(var tile in tiles)
            {
                edges.Add(new Edge(tile.X, tile.Y, EdgeSide.East));
                edges.Add(new Edge(tile.X, tile.Y, EdgeSide.North));
                edges.Add(new Edge(tile.X, tile.Y, EdgeSide.West));

                vertices.Add(new Vertex(tile.X, tile.Y, VertexTileSide.Left));
                vertices.Add(new Vertex(tile.X, tile.Y, VertexTileSide.Right));
            }

            Tiles = tiles.AsReadOnly();
            Edges = edges.AsReadOnly();
            Vertices = vertices.AsReadOnly();

            harbors.Add(new Harbor(MaterialType.Grain, GetEdge(-1, 2, EdgeSide.North), GetTile(-1, 3)));
            harbors.Add(new Harbor(MaterialType.Ore, GetEdge(1, 1, EdgeSide.North), GetTile(1, 2)));
            harbors.Add(new Harbor(MaterialType.Unsourced, GetEdge(-2, 2, EdgeSide.West), GetTile(-3, 3)));
            harbors.Add(new Harbor(MaterialType.Unsourced, GetEdge(2, 0, EdgeSide.East), GetTile(3, 0)));
            harbors.Add(new Harbor(MaterialType.Lumber, GetEdge(-3, 1, EdgeSide.East), GetTile(-3, 1)));
            harbors.Add(new Harbor(MaterialType.Wool, GetEdge(3, -2, EdgeSide.West), GetTile(3, -2)));
            harbors.Add(new Harbor(MaterialType.Brick, GetEdge(-2, -1, EdgeSide.East), GetTile(-2, -1)));
            harbors.Add(new Harbor(MaterialType.Unsourced, GetEdge(2, -3, EdgeSide.West), GetTile(2, -3)));
            harbors.Add(new Harbor(MaterialType.Unsourced, GetEdge(0, -3, EdgeSide.North), GetTile(0, -3)));
            Harbors = harbors.AsReadOnly();
        }

        private ITile GetTile(int x, int y)
        {
            return Tiles.Single(t => t.X == x && t.Y == y);
        }

        private IEdge GetEdge(int x, int y, EdgeSide side)
        {
            return Edges.Single(e => e.X == x && e.Y == y && e.Side == side);
        }
    }
}
