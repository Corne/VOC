using System;
using VOC.Core.Players;

namespace VOC.Core.Items.Achievements
{
    public interface IAchievement
    {
        IPlayer Owner { get; }
        int VictoryPoints { get; }

        event EventHandler<IPlayer> OwnerChanged;

        void Update(IPlayer player);
    }
}