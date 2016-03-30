using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using VOC.Core.Establishments;
using VOC.Core.Items;
using VOC.Core.Items.RawMaterials;
using VOC.Core.Players;

namespace VOC.Core.Boards
{
    public class Board : IBoard
    {
        private static ILog logger = LogManager.GetLogger(nameof(Board));

        private readonly List<IEstablishment> establishments = new List<IEstablishment>();
        private readonly List<IRoad> roads = new List<IRoad>();

        public IEnumerable<ITile> Tiles { get; }
        public IEnumerable<IVertex> Vertices { get; }
        public IEnumerable<IEdge> Edges { get; }
        public IEnumerable<IHarbor> Harbors { get; }
        public IEnumerable<IEstablishment> Establishments { get { return establishments.AsReadOnly(); } }
        public IEnumerable<IRoad> Roads { get { return roads.AsReadOnly(); } }
        public IRobber Robber { get; }
        public Board(IBoardBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.Build();
            Tiles = builder.Tiles;
            Vertices = builder.Vertices;
            Edges = builder.Edges;
            Harbors = builder.Harbors;

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
            var tiles = Tiles.Where(t => t.IsAdjacentTo(vertex));
            if (establishments.Any(e => vertices.Contains(e.Vertex)))
                throw new ArgumentException("Invalid vertex, establishment can't be placed next to another establishment");
            if (tiles.All(t => t.Rawmaterial == MaterialType.Sea))
                throw new ArgumentException("Can't place an establishment on sea!");

            if (establishments.Count(e => e.Owner == owner) >= 2 &&
                roads.Where(r => r.Owner == owner).All(r => !r.Edge.IsAdjacentTo(vertex)))
                throw new InvalidOperationException("Each player can only build 2 houses without adjacent roads!");

            var establishment = new Establishment(owner, vertex);
            establishments.Add(establishment);


            logger.Info($"Establisment Build; Player {owner.Name}, {vertex.ToString()}");

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
            //CvB Todo: fix readability
            if (establishments.All(e => !adjacentVertices.Contains(e.Vertex) || e.Owner != owner) &&
                roads.All(r => !adjacentEdges.Contains(r.Edge) || r.Owner != owner))
                throw new ArgumentException("Road should have an adjacent establisment or road of the player");

            var road = new Road(edge, owner);
            roads.Add(road);

            logger.Info($"Road build; Player {owner.Name}, {edge.ToString()}");

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

        public IEnumerable<IHarbor> GetHarbors(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            var playerEstablisments = Establishments.Where(e => e.Owner == player).ToList();
            return Harbors.Where(harbor => playerEstablisments.Any(e => e.Vertex.IsAdjacentTo(harbor.Edge)));
        }

        public IEnumerable<IPlayer> GetPlayers(ITile tile)
        {
            return GetEstablishments(tile).Select(e => e.Owner).Distinct().ToList();
        }

        public ITile FindTile(int x, int y)
        {
            return Tiles.SingleOrDefault(t => t.X == x && t.Y == y);
        }

        public IVertex FindVertex(int x, int y, VertexTileSide side)
        {
            return Vertices.SingleOrDefault(v => v.X == x && v.Y == y && v.Side == side);
        }

        public IEdge FindEdge(int x, int y, EdgeSide side)
        {
            return Edges.SingleOrDefault(e => e.X == x && e.Y == y && e.Side == side);
        }

        //CvB Todo: Really bad performance wise atm....

        public IEnumerable<IRoad> GetLongestRoad(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            var playerroads = roads.Where(r => r.Owner == player);
            if (!playerroads.Any())
                return new IRoad[0];
            return GetLongestRoad(playerroads);

            //var result = new HashSet<IRoad>();
            //foreach (var item in playerroads)
            //{
            //    var temp = new HashSet<IRoad>();
            //    temp.Add(item);

            //    bool changes = true;
            //    while (changes)
            //    {
            //        changes = false;
            //        var adjacent = playerroads.Where(r1 => temp.Any(r2 => r2.IsAdjacentTo(r1)));
            //        foreach (var road in adjacent)
            //        {
            //            int oldCount = temp.Count;
            //            temp.Add(road);
            //            changes |= temp.Count != oldCount;
            //        }
            //    }

            //    if (temp.Count > result.Count)
            //        result = temp;
            //}

            //return result;
        }

        private IEnumerable<IRoad> GetLongestRoad( IEnumerable<IRoad> roads)
        {
            IEnumerable<IRoad> result = new HashSet<IRoad>();
            foreach (var road in roads)
            {
                var temp = GetLongestRoad(road, roads.Except(new IRoad[] { road }));
                if (temp.Count() > result.Count())
                    result = temp;
            }
            return result;
        }

        private IEnumerable<IRoad> GetLongestRoad(IRoad current, IEnumerable<IRoad> roads)
        {
            var result = new HashSet<IRoad>();
            result.Add(current);

            var adjacentRoads = roads.Where(r => r.IsAdjacentTo(current));

            foreach (var adjacent in adjacentRoads)
            {
                var temp = new List<IRoad>(new IRoad[] { current });
                temp.AddRange(GetLongestRoad(adjacent, roads.Except(adjacentRoads.Union(temp))));

                if (temp.Count > result.Count)
                    result = new HashSet<IRoad>(temp);
            }

            return result;
        }

    }
}
