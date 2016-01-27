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
        private readonly ITurn turn;
        private readonly IEnumerable<IPlayer> players;

        private Dictionary<IPlayer, int> desiredInventorySizes;

        public RobberDiscardState(ITurn turn, IEnumerable<IPlayer> players)
        {
            if (turn == null)
                throw new ArgumentNullException(nameof(turn));
            if (players == null)
                throw new ArgumentNullException(nameof(players));

            this.turn = turn;
            this.players = players;
        }

        public IEnumerable<StateCommand> Commands
        {
            get
            {
                return new StateCommand[] { StateCommand.DiscardResources };
            }
        }

        public void Start()
        {


            desiredInventorySizes = new Dictionary<IPlayer, int>();
            foreach(var player in players)
            {
                player.InventoryChanged -= Player_InventoryChanged;
                if(player.Inventory.Count() > 7)
                {
                    double inveotryCount = player.Inventory.Count();
                    int desired = Convert.ToInt32(Math.Round(inveotryCount/2, MidpointRounding.AwayFromZero));
                    desiredInventorySizes.Add(player, desired);
                    player.InventoryChanged += Player_InventoryChanged;
                }
            }

            CheckStateChange();
        }

        private void Player_InventoryChanged(object sender, EventArgs e)
        {
            if (desiredInventorySizes == null)
                throw new InvalidOperationException("Inventory sizes should never be null when listening to Player Inventory Changes");

            var player = sender as IPlayer;
            if (player == null)
                throw new ArgumentException("Sender should be player");

            if (!desiredInventorySizes.ContainsKey(player))
                throw new InvalidOperationException("No change expected from this player");

            if (player.Inventory.Count() <= desiredInventorySizes[player])
                desiredInventorySizes.Remove(player);

            CheckStateChange();
        }

        private void CheckStateChange()
        {
            if (!desiredInventorySizes.Any())
            {
                Stop();
                turn.SetState<MoveRobberState>();
            }
        }

        public void Stop()
        {
            foreach (var player in players)
            {
                player.InventoryChanged -= Player_InventoryChanged;
            }
        }
    }
}
