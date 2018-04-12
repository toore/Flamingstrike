using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using Player = FlamingStrike.GameEngine.Setup.TerritorySelection.Player;
using Territory = FlamingStrike.GameEngine.Setup.TerritorySelection.Territory;

namespace FlamingStrike.UI.WPF.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModel : IGameboardViewModel, IAlternateGameSetupObserver {}

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

        private Action<Region> _onRegionClick;

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
            set => this.NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x);
        }

        public bool CanUserSelectNumberOfArmies => false;

        public bool CanShowCards => false;

        public void ShowCards() {}

        public void SelectRegion(ITerritorySelector territorySelector)
        {
            UpdateView(
                territorySelector.GetTerritories(),
                territorySelector.PlaceArmyInRegion,
                territorySelector.GetPlayer(),
                territorySelector.GetArmiesLeftToPlace());
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            _eventAggregator.PublishOnUIThread(new StartGameplayMessage(gamePlaySetup));
        }

        private void UpdateView(
            IReadOnlyList<Territory> territories,
            Action<Region> selectAction,
            Player player,
            int armiesToPlace)
        {
            _onRegionClick = selectAction;

            _worldMapViewModelFactory.Update(WorldMapViewModel, Convert(territories), Maybe<Region>.Nothing);

            PlayerName = (string)player.Name;
            PlayerColor = _playerUiDataRepository.Get((string)player.Name).Color;

            InformationText = string.Format(Resources.PLACE_ARMY, armiesToPlace);
        }

        private static IReadOnlyList<Gameplay.Territory> Convert(IEnumerable<Territory> territories)
        {
            return territories.Select(x => new Gameplay.Territory(x.Region, x.IsSelectable, x.Name, x.Armies)).ToList();
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