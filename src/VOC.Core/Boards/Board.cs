using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    public class Board : IBoard
    {
        private readonly List<IEstablishment> establisments;

        public IEnumerable<ITile> Tiles { get; }
        public IEnumerable<IVertex> Vertices { get; }
        public IEnumerable<IEdge> Edges { get; }

        public IEnumerable<IEstablishment> Establisments { get { return establisments.AsReadOnly(); } }

        public Board(IBoardBuilder builder)
        {
            establisments = new List<IEstablishment>();

            //CvB Todo: not sure if correct/clean to call this in constructor
            builder.Build();
            Tiles = builder.Tiles;
            Vertices = builder.Vertices;
            Edges = builder.Edges;
        }

        public IEstablishment BuildEstablisment(IVertex vertex, IPlayer owner)
        {
            if (vertex == null)
                throw new ArgumentNullException(nameof(vertex));

            if (owner == null)
                throw new ArgumentNullException(nameof(owner));

            if (!Vertices.Contains(vertex))
                throw new ArgumentException("Did not find the passed vertex on the board");

            if (establisments.Any(e => e.Vertex == vertex))
                throw new ArgumentException("Invalid vertex, already an establisment here");

            var tiles = Tiles.Where(t => t.IsAdjacentTo(vertex));
            if (tiles.All(t => t.Rawmaterial == MaterialType.Sea))
                throw new ArgumentException("Can't place an establilsment on sea!");

            var establisment = new Establishment(owner, vertex);
            establisments.Add(establisment);
            return establisment;
        }

        public IEnumerable<IEstablishment> GetEstablishments(ITile tile)
        {
            if (tile == null)
                throw new ArgumentNullException(nameof(tile));

            return Establisments.Where(e => e.Vertex.IsAdjacentTo(tile)).ToList();
        }

        public IEnumerable<ITile> GetTiles(int number)
        {
            return Tiles.Where(t => t.Number == number).ToList();
        }
    }
}
