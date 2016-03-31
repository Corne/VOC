using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Players;

namespace VOC.Core.Items.Achievements
{
    /// <summary>
    /// Biggest Army actives for first player that has 3 or more knights cards played. 
    /// Worth 2 victory points
    /// If another player has more played knights, that player will take over this achievement
    /// </summary>
    public class BiggestArmy : IAchievement
    {
        private IPlayer _owner;
        public IPlayer Owner
        {
            get { return _owner; }
            private set { _owner = value; OwnerChanged?.Invoke(this, Owner); }
        }

        public int VictoryPoints { get { return 2; } }
        public event EventHandler<IPlayer> OwnerChanged;

        public void Update(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            if (Owner == null && player.ArmySize >= 3)
                Owner = player;
            else if (Owner != null && player.ArmySize > Owner.ArmySize)
                Owner = player;
        }
    }
}
