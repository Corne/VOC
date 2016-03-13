using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Players;

namespace VOC.Core.Games
{
    //www.spelregels-online.nl/j-k-l/de-kolonisten-van-catan/de-kolonisten-van-catan
    public class Game : IGame
    {
        private readonly ISet<IPlayer> players;
        private readonly ITurnProvider provider;
        private ITurn currentTurn;

        public Game(ISet<IPlayer> players, ITurnProvider provider)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (players.Count < 2 || players.Count > 4)
                throw new ArgumentException("Number of players should be between 2 and 4");

            this.players = players;
            this.provider = provider;
        }

        public IEnumerable<IPlayer> Players { get { return players.ToList().AsReadOnly(); } }

        public event EventHandler<ITurn> TurnStarted;


        public void Start()
        {
            if (currentTurn != null) return;

            currentTurn = provider.GetNext();
            currentTurn.Ended += CurrentTurn_Ended;
            TurnStarted?.Invoke(this, currentTurn);
        }

        private void CurrentTurn_Ended(object sender, EventArgs e)
        {
            currentTurn.Ended -= CurrentTurn_Ended;
            currentTurn = provider.GetNext();
            currentTurn.Ended += CurrentTurn_Ended;
            TurnStarted?.Invoke(this, currentTurn);
        }

        public void Execute(IPlayerCommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (currentTurn == null)
                throw new InvalidOperationException("Game not started");

            if (command.Player != currentTurn.Player && command.Type != GameCommand.Trade) 
                throw new InvalidOperationException("This player can't execute a command at the moment");
            if (!currentTurn.CanExecute(command.Type))
                throw new ArgumentException("Can't execute this command in current state");

            command.Execute();
            currentTurn.AfterExecute(command.Type);
        }
    }
}
