using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class TurnProvider : ITurnProvider
    {
        private enum ProviderState { HighRoll, Build, Game }

        private readonly ISet<IPlayer> players;
        private readonly ITurnFactory factory;
        private readonly List<IHighRollTurn> highrollresults = new List<IHighRollTurn>();
        private ProviderState state = ProviderState.HighRoll;
        private Queue<IPlayer> playerQueue;

        public TurnProvider(ISet<IPlayer> players, ITurnFactory factory)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            if (players.Count < 2)
                throw new ArgumentException("Turn provider expects at least 2 players");
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));
            this.players = players;
            this.factory = factory;
        }

        public ITurn GetNext()
        {
            switch (state)
            {
                case ProviderState.HighRoll:
                    return GetNextHighRoll();
                case ProviderState.Build:
                    return GetNextBuildturn();
                default:
                    return GetNextGameTurn();
            }
        }

        private IHighRollTurn GetNextHighRoll()
        {
            if (playerQueue == null)
                playerQueue = new Queue<IPlayer>(players);

            var turn = factory.Create<IHighRollTurn>(playerQueue.Dequeue());
            highrollresults.Add(turn);
            //CvB Todo: we should reset when multiple with highest value?
            if (!playerQueue.Any())
            {
                OrderPlayersByHighRollResult();
                state = ProviderState.Build;
            }

            return turn;
        }

        private void OrderPlayersByHighRollResult()
        {
            players.Clear();
            var max = highrollresults.Aggregate((r1, r2) => r1.Result >= r2.Result ? r1 : r2);
            foreach (var player in highrollresults.SkipWhile(r => r != max).Concat(highrollresults.TakeWhile(r => r != max)))
            {
                players.Add(player.Player);
            }
        }

        private IBuildTurn GetNextBuildturn()
        {
            if (!playerQueue.Any())
                playerQueue = new Queue<IPlayer>(players.Concat(players));

            var turn = factory.Create<IBuildTurn>(playerQueue.Dequeue());

            if (!playerQueue.Any())
                state = ProviderState.Game;

            return turn;
        }

        private IGameTurn GetNextGameTurn()
        {
            if (!playerQueue.Any())
                playerQueue = new Queue<IPlayer>(players);

            return factory.Create<IGameTurn>(playerQueue.Dequeue());
        }
    }
}
