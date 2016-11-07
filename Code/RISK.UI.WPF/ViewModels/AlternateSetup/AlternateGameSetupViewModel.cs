using System;
using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Messages;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModel : IMainViewModel, IAlternateGameSetupObserver {}

    public class AlternateGameSetupViewModel : Screen, IAlternateGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private string _informationText;
        private string _playerName;
        private Color _playerColor;

        public AlternateGameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            WorldMapViewModel = _worldMapViewModelFactory.Create(x => _onRegionClick(x));
        }

        private Action<IRegion> _onRegionClick;

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText
        {
            get { return _informationText; }
            private set { this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        public string PlayerName
        {
            get { return _playerName; }
            private set { this.NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public Color PlayerColor
        {
            get { return _playerColor; }
            set { this.NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x); }
        }

        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            UpdateView(
                placeArmyRegionSelector.Territories,
                region => placeArmyRegionSelector.PlaceArmyInRegion(region),
                placeArmyRegionSelector.SelectableRegions,
                placeArmyRegionSelector.Player,
                placeArmyRegionSelector.GetArmiesLeftToPlace());
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gamePlaySetup));
        }

        private void UpdateView(IReadOnlyList<ITerritory> territories, Action<IRegion> selectAction, IReadOnlyList<IRegion> enabledRegions, IPlayer player, int armiesToPlace)
        {
            _onRegionClick = selectAction;

            _worldMapViewModelFactory.Update(WorldMapViewModel, territories, enabledRegions, Maybe<IRegion>.Nothing);

            PlayerName = player.Name;
            PlayerColor = _playerUiDataRepository.Get(player).Color;

            InformationText = string.Format(Resources.PLACE_ARMY, armiesToPlace);
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