using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;

namespace FlamingStrike.UI.WPF.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModel : IGameboardViewModel {}

    public class AlternateGameSetupViewModel : Screen, IAlternateGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IGameEngineClient _gameEngineClient;
        private string _informationText;
        private string _playerName;
        private Color _playerColor;
        private Action<Region> _onRegionClick;
        private IDisposable _selectRegionSubscription;
        private IDisposable _gamePlaySetupSubscription;

        public AlternateGameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IGameEngineClient gameEngineClient)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _gameEngineClient = gameEngineClient;

            WorldMapViewModel = _worldMapViewModelFactory.Create(x => _onRegionClick(x));
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText
        {
            get => _informationText;
            private set => this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x);
        }

        public string PlayerName
        {
            get => _playerName;
            private set => this.NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x);
        }

        public Color PlayerColor
        {
            get => _playerColor;
            private set => this.NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x);
        }

        public bool CanUserSelectNumberOfArmies => false;

        public bool CanShowCards => false;

        public void ShowCards() {}

        protected override void OnActivate()
        {
            base.OnActivate();

            _selectRegionSubscription = _gameEngineClient.OnSelectRegion.Subscribe(SelectRegion);
            _gamePlaySetupSubscription = _gameEngineClient.OnGamePlaySetup.Subscribe(NewGamePlaySetup);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            _selectRegionSubscription.Dispose();
            _gamePlaySetupSubscription.Dispose();
        }

        private void SelectRegion(ITerritorySelector territorySelector)
        {
            UpdateView(
                territorySelector.Territories,
                territorySelector.Player,
                territorySelector.PlaceArmyInRegion,
                territorySelector.ArmiesLeftToPlace);
        }

        private void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gamePlaySetup));
        }

        private void UpdateView(
            IEnumerable<Territory> territories,
            string player,
            Action<Region> onRegionClick,
            int armiesToPlace)
        {
            _worldMapViewModelFactory.Update(WorldMapViewModel, Convert(territories), Maybe<Region>.Nothing);

            _onRegionClick = onRegionClick;

            PlayerName = player;
            PlayerColor = _playerUiDataRepository.Get(player).Color;

            InformationText = string.Format(Resources.PLACE_ARMY, armiesToPlace);
        }

        private static IReadOnlyList<Gameplay.Territory> Convert(IEnumerable<Territory> territories)
        {
            return territories.Select(x => new Gameplay.Territory(x.Region, x.IsSelectable, x.Player, x.Armies)).ToList();
        }

        public bool CanEnterFortifyMode => false;

        public void EnterFortifyMode() {}

        public bool CanEnterAttackMode => false;

        public void EnterAttackMode() {}

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