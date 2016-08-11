using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModel : IMainViewModel
    {
        void UpdateView(IReadOnlyList<ITerritory> territories, Action<IRegion> selectTerritoryAction, IEnumerable<IRegion> enabledTerritories, string playerName, int armiesLeftToPlace);
    }

    public class AlternateGameSetupViewModel : Screen, IAlternateGameSetupViewModel, IGameboardViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly ITaskEx _taskEx;

        public AlternateGameSetupViewModel(
            IGameFactory gameFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IAlternateGameSetup alternateGameSetup,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactory = gameFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _alternateGameSetup = alternateGameSetup;
            _taskEx = taskEx;
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { this.NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

        private string _informationText;
        public string InformationText
        {
            get { return _informationText; }
            private set { this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        private string _playerName;
        public string PlayerName
        {
            get { return _playerName; }
            private set { this.NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public bool CanEnterFortifyMode => false;

        public void Activate()
        {
            OnActivate();
        }

        protected override async void OnActivate()
        {
            var gamePlaySetup = await SetupGame();

            var game = _gameFactory.Create(gamePlaySetup);
            StartGameplay(game);
        }

        private async Task<IGamePlaySetup> SetupGame()
        {
            var setupOfGameAsync = SetupOfGameAsync();
            var gamePlaySetup = await setupOfGameAsync;

            return gamePlaySetup;
        }

        private async Task<IGamePlaySetup> SetupOfGameAsync()
        {
            IGamePlaySetup gameSetup = null;
            await _taskEx.Run(() => { gameSetup = _alternateGameSetup.Initialize(); });

            return gameSetup;
        }

        private void StartGameplay(IGame game)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(game));
        }

        public void UpdateView(IReadOnlyList<ITerritory> territories, Action<IRegion> selectTerritoryAction, IEnumerable<IRegion> enabledTerritories, string playerName, int armiesLeftToPlace)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(
                territories,
                selectTerritoryAction,
                enabledTerritories);

            WorldMapViewModel = worldMapViewModel;

            PlayerName = playerName;

            InformationText = string.Format(ResourceManager.Instance.GetString("PLACE_ARMY"), armiesLeftToPlace);
        }

        public void EnterFortifyMode() {}
        public bool CanEndTurn => false;

        public void EndTurn() {}

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }
    }
}