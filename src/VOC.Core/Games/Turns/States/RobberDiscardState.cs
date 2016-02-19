using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns.States
{
    public class RobberDiscardState : ITurnState
    {
        private readonly IGameTurn turn;
        private readonly IEnumerable<IPlayer> players;

        private Dictionary<IPlayer, int> desiredInventorySizes;

        public RobberDiscardState(IGameTurn turn, IEnumerable<IPlayer> players)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (players == null)
                throw new ArgumentNullException(nameof(players));

            this.turn = turn;
            this.players = players;


            desiredInventorySizes = new Dictionary<IPlayer, int>();
            foreach (var player in players)
            {
                if (player.Inventory.Count() > 7)
                {
                    double inveotryCount = player.Inventory.Count();
                    int desired = Convert.ToInt32(Math.Round(inveotryCount / 2, MidpointRounding.AwayFromZero));
                    desiredInventorySizes.Add(player, desired);
                }
            }

            CheckStateChange();
        }

        public IEnumerable<GameCommand> Commands
        {
            get
            {
                return new GameCommand[] { GameCommand.DiscardResources };
            }
        }

        private void CheckStateChange()
        {
            if (!desiredInventorySizes.Any())
            {
                turn.SetState<MoveRobberState>();
            }
        }


        public void AfterExecute(GameCommand command)
        {
            if (command != GameCommand.DiscardResources)
                return;

            var removeList = new List<IPlayer>();
            foreach (var player in desiredInventorySizes)
            {
                if (player.Key.Inventory.Count() <= player.Value)
                    removeList.Add(player.Key);
            }
            foreach (var player in removeList)
            {
                desiredInventorySizes.Remove(player);
            }
            CheckStateChange();
        }
    }
}
