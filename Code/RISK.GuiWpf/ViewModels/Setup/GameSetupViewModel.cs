using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using RISK.Application.Play;
using RISK.Application.Setup;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainViewModel { }

    public class GameSetupViewModel : Screen, ITerritoryRequestHandler, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactory _gameFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly IGuiThreadDispatcher _guiThreadDispatcher;
        private readonly ITaskEx _taskEx;

        public GameSetupViewModel(
            IGameFactory gameFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IAlternateGameSetup alternateGameSetup,
            IGuiThreadDispatcher guiThreadDispatcher,
            ITaskEx taskEx)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactory = gameFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _alternateGameSetup = alternateGameSetup;
            _guiThreadDispatcher = guiThreadDispatcher;
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
            var territoryRequestHandler = this;

            IGamePlaySetup gameSetup = null;
            await _taskEx.Run(() =>
            {
                gameSetup = _alternateGameSetup.Initialize(territoryRequestHandler);
            });

            return gameSetup;
        }

        public ITerritoryId ProcessRequest(ITerritoryRequestParameter territoryRequestParameter)
        {
            var userInteractor = new UserInteractor();
            _guiThreadDispatcher.Invoke(() => UpdateView(territoryRequestParameter.Territories, userInteractor.SelectTerritory, territoryRequestParameter.EnabledTerritories, territoryRequestParameter.PlayerId.Name, territoryRequestParameter.GetArmiesLeftToPlace()));

            return userInteractor.WaitForTerritoryToBeSelected(territoryRequestParameter);
        }

        private void StartGameplay(IGame game)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(game));
        }

        private void UpdateView(IReadOnlyList<ITerritory> territories, Action<ITerritoryId> selectTerritoryAction, IReadOnlyList<ITerritoryId> enabledTerritories, string playerName, int armiesLeftToPlace)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(
                territories,
                selectTerritoryAction,
                enabledTerritories);

            WorldMapViewModel = worldMapViewModel;

            PlayerName = playerName;

            InformationText = string.Format(Resources.PLACE_ARMY, armiesLeftToPlace);
        }

        public bool CanFortify()
        {
            return false;
        }

        public void Fortify() { }

        public bool CanEndTurn()
        {
            return false;
        }

        public void EndTurn() { }

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