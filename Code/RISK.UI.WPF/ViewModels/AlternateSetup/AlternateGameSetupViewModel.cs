using System;
using System.Collections.Generic;
using Caliburn.Micro;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Messages;

namespace RISK.UI.WPF.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModel : IMainViewModel, IAlternateGameSetupObserver {}

    public class AlternateGameSetupViewModel : Screen, IAlternateGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private WorldMapViewModel _worldMapViewModel;
        private string _informationText;
        private string _playerName;

        public AlternateGameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { this.NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

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

        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            UpdateView(
                placeArmyRegionSelector.Territories,
                placeArmyRegionSelector.PlaceArmyInRegion,
                placeArmyRegionSelector.SelectableRegions,
                placeArmyRegionSelector.PlayerName,
                placeArmyRegionSelector.GetArmiesLeftToPlace());
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gamePlaySetup));
        }

        private void UpdateView(IReadOnlyList<ITerritory> territories, Action<IRegion> selectAction, IReadOnlyList<IRegion> enabledRegions, string playerName, int armiesLeftToPlace)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(selectAction);
            _worldMapViewModelFactory.Update(worldMapViewModel, territories, enabledRegions, null);

            WorldMapViewModel = worldMapViewModel;

            PlayerName = playerName;

            InformationText = string.Format(Resources.PLACE_ARMY, armiesLeftToPlace);
        }

        public bool CanEnterFortifyMode => false;

        public bool CanEnterAttackMode => false;

        public bool CanEndTurn => false;

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