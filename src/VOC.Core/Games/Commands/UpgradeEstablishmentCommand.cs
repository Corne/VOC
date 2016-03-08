using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Establishments;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games.Commands
{
    public class UpgradeEstablishmentCommand : IPlayerCommand
    {
        private readonly IEstablishment establishment;
        public UpgradeEstablishmentCommand(IPlayer player, IEstablishment establishment)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            if (establishment == null)
                throw new ArgumentNullException(nameof(establishment));
            Player = player;
            this.establishment = establishment;
        }

        public IPlayer Player { get; }

        public GameCommand Type { get { return GameCommand.UpdgradeEstablisment; } }

        public void Execute()
        {
            if (!Player.HasResources(Establishment.UPGRADE_RESOURCES))
                throw new InvalidOperationException("Can't upgrade Establisment because player doesn't have the resources!");

            establishment.Upgrade();
            Player.TakeResources(Establishment.UPGRADE_RESOURCES);
        }
    }
}
