using System;
using VOC.Core.Players;

namespace VOC.Core.Games.Turns
{
    public class BuildTurn : IBuildTurn
    {
        private object buildLock = new object();
        private GameCommand expectedCommand = GameCommand.BuildEstablisment;
        public BuildTurn(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            Player = player;
        }

        public IPlayer Player { get; }
        public event EventHandler Ended;

        public void AfterExecute(GameCommand command)
        {
            lock (buildLock)
            {
                if (!CanExecute(command)) return;

                switch (command)
                {
                    case GameCommand.BuildEstablisment:
                        expectedCommand = GameCommand.BuildRoad;
                        break;
                    case GameCommand.BuildRoad:
                        expectedCommand = GameCommand.NextState;
                        Ended?.Invoke(this, EventArgs.Empty);
                        break;
                }
            }
        }

        public bool CanExecute(GameCommand command)
        {
            return command == expectedCommand;
        }
    }
}
