using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOC.Core.Games.Commands;
using VOC.Core.Games.Turns;
using VOC.Core.Items.Cards;
using VOC.Core.Players;
using VOC.Core.Trading;

namespace VOC.Core.Games
{
    //www.spelregels-online.nl/j-k-l/de-kolonisten-van-catan/de-kolonisten-van-catan
    public class Game : IGame
    {
        private readonly ISet<IPlayer> players;
        private readonly ITurnProvider provider;
        private readonly IBank bank;
        private ITurn currentTurn;

        public Game(ISet<IPlayer> players, ITurnProvider provider, IBank bank)
        {
            if (players == null)
                throw new ArgumentNullException(nameof(players));
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));
            if (bank == null)
                throw new ArgumentNullException(nameof(bank));
            if (players.Count < 2 || players.Count > 4)
                throw new ArgumentException("Number of players should be between 2 and 4");

            this.players = players;
            this.provider = provider;
            this.bank = bank;
        }

        public IEnumerable<IPlayer> Players { get { return players.ToList().AsReadOnly(); } }

        public event EventHandler<ITurn> TurnStarted;
        public event EventHandler<IPlayer> Finished;

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

            bank.UpdateAchievements(currentTurn.Player);
            if (bank.VerifyWinCondition(currentTurn.Player))
                SetGameFinished();
            else
                currentTurn.AfterExecute(command.Type);
        }

        private void SetGameFinished()
        {
            currentTurn.Ended -= CurrentTurn_Ended;
            var player = currentTurn.Player;
            currentTurn = null;
            Finished?.Invoke(this, player);
        }

        public void PlayDevelopmentCard(IDevelopmentCard card)
        {
            var gameturn = currentTurn as IGameTurn;
            if (gameturn == null)
                throw new InvalidOperationException("Can't play a development card in current turn");
            gameturn.PlayDevelopmentCard(card);
        }

        public IPlayer FindPlayer(Guid id)
        {
            return Players.FirstOrDefault(p => p.Id == id);
        }

        public void BuyDevelopmentCard(IPlayer player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            //Cvb Todo: not sure if this check is correct, maybe should check on turn.canexecute???
            var gameturn = currentTurn as IGameTurn;
            if (gameturn == null)
                throw new InvalidOperationException("Game is not in a state to buy developmentcards");
            if (player != currentTurn.Player)
                throw new InvalidOperationException("Only player who has his turn can buy cards");

            bank.BuyDevelopmentCard(player, currentTurn);
        }
    }
}
