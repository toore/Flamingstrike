using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IGameplayViewModel : IGameboardViewModel, IGameObserver
    {
        IList<PlayerStatusViewModel> PlayerStatuses { get; }
        int NumberOfUserSelectedArmies { get; set; }
        int MaximumUserSelectableArmies { get; }
    }

    public class GameplayViewModel :
        ViewModelBase,
        IGameplayViewModel,
        ISelectAttackingRegionInteractionStateObserver,
        IAttackInteractionStateObserver,
        ISelectSourceRegionForFortificationInteractionStateObserver,
        IFortifyInteractionStateObserver
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerStatusViewModelFactory _playerStatusViewModelFactory;
        private string _informationText;
        private string _playerName;
        private Color _playerColor;
        private bool _canEnterFortifyMode;
        private bool _canEnterAttackMode;
        private bool _canEndTurn;
        private IInteractionState _interactionState;
        private IGameStatus _gameStatus;
        private IAttackPhase _attackPhase;
        private IList<PlayerStatusViewModel> _playerStatuses;
        private int _numberOfArmies;
        private int _maximumUserSelectableArmies;
        private Maybe<IRegion> _previouslySelectedAttackingRegion;
        private bool _canUserSelectNumberOfArmies;

        public GameplayViewModel(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IPlayerStatusViewModelFactory playerStatusViewModelFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _playerStatusViewModelFactory = playerStatusViewModelFactory;

            WorldMapViewModel = _worldMapViewModelFactory.Create(OnRegionClicked);
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText
        {
            get => _informationText;
            private set { NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        public string PlayerName
        {
            get => _playerName;
            private set { NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public Color PlayerColor
        {
            get => _playerColor;
            private set { NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x); }
        }

        public bool CanEnterFortifyMode
        {
            get => _canEnterFortifyMode;
            private set { NotifyOfPropertyChange(value, () => CanEnterFortifyMode, x => _canEnterFortifyMode = x); }
        }

        public bool CanEnterAttackMode
        {
            get => _canEnterAttackMode;
            private set { NotifyOfPropertyChange(value, () => CanEnterAttackMode, x => _canEnterAttackMode = x); }
        }

        public bool CanEndTurn
        {
            get => _canEndTurn;
            private set { NotifyOfPropertyChange(value, () => CanEndTurn, x => _canEndTurn = x); }
        }

        public IList<PlayerStatusViewModel> PlayerStatuses
        {
            get => _playerStatuses;
            private set { NotifyOfPropertyChange(value, () => PlayerStatuses, x => _playerStatuses = x); }
        }

        public int NumberOfUserSelectedArmies
        {
            get => _numberOfArmies;
            set => _numberOfArmies = value;
        }

        public int MaximumUserSelectableArmies
        {
            get => _maximumUserSelectableArmies;
            private set => NotifyOfPropertyChange(value, () => MaximumUserSelectableArmies, x => _maximumUserSelectableArmies = x);
        }

        public bool CanUserSelectNumberOfArmies
        {
            get => _canUserSelectNumberOfArmies;
            private set => NotifyOfPropertyChange(value, () => CanUserSelectNumberOfArmies, x => _canUserSelectNumberOfArmies = x);
        }

        public bool CanShowCards => true;

        public void DraftArmies(IGameStatus gameStatus, IDraftArmiesPhase draftArmiesPhase)
        {
            _previouslySelectedAttackingRegion = Maybe<IRegion>.Nothing;
            UpdatePlayersInformation(gameStatus);

            ShowDraftArmiesView(gameStatus, draftArmiesPhase);
        }

        public void Attack(IGameStatus gameStatus, IAttackPhase attackPhase)
        {
            _gameStatus = gameStatus;
            _attackPhase = attackPhase;

            UpdatePlayersInformation(gameStatus);

            _previouslySelectedAttackingRegion.End(
                selectedRegion => ShowAttackPhaseView(gameStatus, attackPhase, selectedRegion),
                () => ShowAttackPhaseView(gameStatus, attackPhase));
        }

        public void SendArmiesToOccupy(IGameStatus gameStatus, ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            UpdatePlayersInformation(gameStatus);

            ShowSendArmiesToOccupyView(gameStatus, sendArmiesToOccupyPhase);
        }

        public void EndTurn(IGameStatus gameStatus, IEndTurnPhase endTurnPhase)
        {
            UpdatePlayersInformation(gameStatus);

            ShowEndTurnView(gameStatus, endTurnPhase);
        }

        public void GameOver(IGameOverState gameOverState)
        {
            ShowGameOverMessage(gameOverState.Winner);
        }

        public void EnterAttackMode()
        {
            ShowAttackPhaseView(_gameStatus, _attackPhase);
        }

        void ISelectAttackingRegionInteractionStateObserver.Select(IRegion selectedRegion)
        {
            _previouslySelectedAttackingRegion = Maybe<IRegion>.Create(selectedRegion);

            ShowAttackPhaseView(_gameStatus, _attackPhase, selectedRegion);
        }

        void IAttackInteractionStateObserver.DeselectRegion()
        {
            ShowAttackPhaseView(_gameStatus, _attackPhase);
        }

        public void ShowCards() {}

        public void EnterFortifyMode()
        {
            ShowFortifyView(_gameStatus, _attackPhase);
        }

        void ISelectSourceRegionForFortificationInteractionStateObserver.Select(IRegion selectedRegion)
        {
            ShowFortifyView(_gameStatus, _attackPhase, selectedRegion);
        }

        void IFortifyInteractionStateObserver.DeselectRegion()
        {
            ShowFortifyView(_gameStatus, _attackPhase);
        }

        public void EndTurn()
        {
            _interactionState.EndTurn();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        private void OnRegionClicked(IRegion region)
        {
            _interactionState.OnRegionClicked(region);
        }

        private void ShowDraftArmiesView(IGameStatus gameStatus, IDraftArmiesPhase draftArmiesPhase)
        {
            _interactionState = _interactionStateFactory.CreateDraftArmiesInteractionState(draftArmiesPhase);

            UpdateView(gameStatus.Territories);
        }

        private void ShowAttackPhaseView(IGameStatus gameStatus, IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(attackPhase, this);

            UpdateView(gameStatus.Territories);
        }

        private void ShowAttackPhaseView(IGameStatus gameStatus, IAttackPhase attackPhase, IRegion selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateAttackInteractionState(attackPhase, selectedRegion, this);

            UpdateView(gameStatus.Territories);
        }

        private void ShowFortifyView(IGameStatus gameStatus, IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(attackPhase, this);

            UpdateView(gameStatus.Territories);
        }

        private void ShowFortifyView(IGameStatus gameStatus, IAttackPhase attackPhase, IRegion selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateFortifyInteractionState(attackPhase, selectedRegion, this);

            UpdateView(gameStatus.Territories);
        }

        private void ShowSendArmiesToOccupyView(IGameStatus gameStatus, ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _interactionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);

            UpdateView(gameStatus.Territories);
        }

        private void ShowEndTurnView(IGameStatus gameStatus, IEndTurnPhase endTurnPhase)
        {
            _interactionState = _interactionStateFactory.CreateEndTurnInteractionState(endTurnPhase);

            UpdateView(gameStatus.Territories);
        }

        private void ShowGameOverMessage(IPlayer winner)
        {
            _dialogManager.ShowGameOverDialog(winner.Name);

            _eventAggregator.PublishOnUIThread(new NewGameMessage());
        }

        private void UpdatePlayersInformation(IGameStatus gameStatus)
        {
            PlayerName = gameStatus.CurrentPlayer.Name;
            PlayerColor = _playerUiDataRepository.Get(gameStatus.CurrentPlayer).Color;

            PlayerStatuses = gameStatus.PlayerGameDatas
                .Select(x => _playerStatusViewModelFactory.Create(x))
                .ToList();
        }

        private void UpdateView(IReadOnlyList<ITerritory> territories)
        {
            InformationText = _interactionState.Title;
            CanEnterFortifyMode = _interactionState.CanEnterFortifyMode;
            CanEnterAttackMode = _interactionState.CanEnterAttackMode;
            CanEndTurn = _interactionState.CanEndTurn;

            _worldMapViewModelFactory.Update(
                WorldMapViewModel,
                territories,
                _interactionState.EnabledRegions,
                _interactionState.SelectedRegion);
        }
    }
}