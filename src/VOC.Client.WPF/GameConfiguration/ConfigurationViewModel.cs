using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using VOC.Client.Dashboard.Configuration;
using VOC.Client.WPF.Main;

namespace VOC.Client.WPF.Configuration
{
    public class ConfigurationViewModel : ViewModelBase, IContentViewModel
    {
        private readonly IMapConfigurator mapConfigurator;
        private readonly IGameConfigurator gameConfigurator;
        private readonly RelayCommand _startgameCommand;

        public ConfigurationViewModel(IGameConfigurator gameConfigurator, IMapConfigurator mapConfigurator)
        {
            if (gameConfigurator == null)
                throw new ArgumentNullException(nameof(gameConfigurator));
            if (mapConfigurator == null)
                throw new ArgumentNullException(nameof(mapConfigurator));
            this.mapConfigurator = mapConfigurator;
            this.gameConfigurator = gameConfigurator;
            _startgameCommand = new RelayCommand(StartGame, () => CanStartGame);
        }

        private int _port = 1337;
        public int Port
        {
            get { return _port; }
            set
            {
                if (Set(ref _port, value))
                    _startgameCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<IMap> Maps { get; } = new ObservableCollection<IMap>();

        private IMap _selectedMap;
        public IMap SelectedMap
        {
            get { return _selectedMap; }
            set
            {
                if (Set(ref _selectedMap, value))
                {
                    if (SelectedMap == null)
                        Players = new int[0];
                    else
                        Players = Enumerable.Range(SelectedMap.MinPlayers, SelectedMap.MaxPlayers - SelectedMap.MinPlayers + 1).ToList();
                    _startgameCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private IList<int> _players = new int[0];
        public IList<int> Players
        {
            get { return _players; }
            private set
            {
                if (Set(ref _players, value ?? new int[0]) && !Players.Contains(SelectedPlayerCount))
                    SelectedPlayerCount = _players.FirstOrDefault();
            }
        }

        private int _selectedPlayerCount;
        public int SelectedPlayerCount
        {
            get { return _selectedPlayerCount; }
            set
            {
                if (Set(ref _selectedPlayerCount, value))
                    _startgameCommand.RaiseCanExecuteChanged();
            }
        }

        public ICommand StartGameCommand
        {
            get { return _startgameCommand; }
        }

        private bool CanStartGame
        {
            get
            {
                return Port > 0
                    && SelectedMap != null
                    && SelectedPlayerCount >= SelectedMap.MinPlayers
                    && SelectedPlayerCount <= SelectedMap.MaxPlayers;
            }
        }

        private void StartGame()
        {
            if (!CanStartGame) return;

            var configuration = new GameConfiguration(SelectedMap, SelectedPlayerCount);
            gameConfigurator.Start(configuration, Port);
        }

        public Task OnClose()
        {
            return Task.FromResult(0);
        }

        public async Task OnNavigate()
        {
            var maps = await mapConfigurator.GetMaps();
            foreach (var map in maps)
                Maps.Add(map);
            SelectedMap = Maps.FirstOrDefault();
        }
    }
}
