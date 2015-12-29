using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    public class Board : IBoard
    {
        private readonly List<IEstablishment> establishments;
        private readonly List<IRoad> roads;

        public IEnumerable<ITile> Tiles { get; }
        public IEnumerable<IVertex> Vertices { get; }
        public IEnumerable<IEdge> Edges { get; }

        public IEnumerable<IEstablishment> Establishments { get { return establishments.AsReadOnly(); } }
        public IEnumerable<IRoad> Roads { get { return roads.AsReadOnly(); } }
        public IRobber Robber { get; }
        public Board(IBoardBuilder builder)
        {
            establishments = new List<IEstablishment>();
            roads = new List<IRoad>();

            builder.Build();
            Tiles = builder.Tiles;
            Vertices = builder.Vertices;
            Edges = builder.Edges;

            Robber = new Robber(Tiles.Single(t => t.Rawmaterial == MaterialType.Unsourced));
        }

        public IEstablishment BuildEstablishment(IVertex vertex, IPlayer owner)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (!Vertices.Contains(vertex))
                throw new ArgumentException("Did not find the passed vertex on the board");

            if (establishments.Any(e => e.Vertex == vertex))
                throw new ArgumentException("Invalid vertex, already an establishment here");

            var vertices = Vertices.Where(v => v.IsAdjacentTo(vertex));
            if (establishments.Any(e => vertices.Contains(e.Vertex)))
                throw new ArgumentException("Invalid vertex, establishment can't be placed next to another establishment");

            var tiles = Tiles.Where(t => t.IsAdjacentTo(vertex));
            if (tiles.All(t => t.Rawmaterial == MaterialType.Sea))
                throw new ArgumentException("Can't place an establishment on sea!");

            var establishment = new Establishment(owner, vertex);
            establishments.Add(establishment);
            return establishment;
        }

        public IRoad BuildRoad(IEdge edge, IPlayer owner)
        {
            if (edge == null)
                throw new ArgumentNullException(nameof(edge));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (!Edges.Contains(edge))
                throw new ArgumentException("Edge does not exist on the board");

            if (roads.Any(r => r.Edge == edge))
                throw new ArgumentException("There already is a road build on the given edge");

            if (Tiles.Where(t => t.IsAdjacentTo(edge)).All(t => t.Rawmaterial == MaterialType.Sea))
                throw new ArgumentException("Can't build roads on sea!");

            var adjacentVertices = Vertices.Where(v => v.IsAdjacentTo(edge));
            var adjacentEdges = Edges.Where(e => e.IsAdjacentTo(edge));
            //CvB Todo: not really readable
            if (establishments.All(e => !adjacentVertices.Contains(e.Vertex) || e.Owner != owner) &&
                roads.All(r => !adjacentEdges.Contains(r.Edge) || r.Owner != owner))
                throw new ArgumentException("Road should have an adjacent establisment or road of the player");

            var road = new Road(edge, owner);
            roads.Add(road);
            return road;
        }

        public IEnumerable<IEstablishment> GetEstablishments(ITile tile)
        {
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            return Establishments.Where(e => e.Vertex.IsAdjacentTo(tile)).ToList();
        }

        public IEnumerable<ITile> GetResourceTiles(int number)
        {
            return Tiles.Except(new[] { Robber.CurrentTile }).Where(t => t.Number == number).ToList();
        }
    }
}
