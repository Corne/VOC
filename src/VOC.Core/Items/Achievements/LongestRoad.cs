using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Boards;
using VOC.Core.Players;

namespace VOC.Core.Items.Achievements
{
    /// <summary>
    /// Longest Road will be given to first player with 5 connecting roads, not counting forks
    /// If another player builds a longer road he will take the achievement
    /// longest road is 2 victorypoints
    /// </summary>
    public class LongestRoad : IAchievement
    {
        private readonly IBoard board;
        public LongestRoad(IBoard board)
        {
            if (board == null)
                throw new ArgumentNullException(nameof(board));
            this.board = board;
        }
        private IPlayer _owner;
        public IPlayer Owner
        {
            get { return _owner; }
            set { _owner = value; OwnerChanged?.Invoke(this, Owner); }
        }

        public int VictoryPoints { get { return 2; } }

        public event EventHandler<IPlayer> OwnerChanged;

        public void Update(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            //Don't have to check if the current owner is equal to the player
            if (Owner == player)
                return;

            var longestroadSize = board.GetLongestRoad(player).Count();
            if (Owner == null && longestroadSize >= 5)
                Owner = player;
            else if (Owner != null && longestroadSize > board.GetLongestRoad(Owner).Count())
                Owner = player;
        }
    }
}
