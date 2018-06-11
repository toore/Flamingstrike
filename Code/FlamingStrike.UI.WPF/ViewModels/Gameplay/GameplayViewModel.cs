using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay
{
    public interface IGameplayViewModel : IGameboardViewModel
    {
        IList<PlayerStatusViewModel> PlayerStatuses { get; }
        int NumberOfUserSelectedArmies { get; set; }
        int MaxNumberOfUserSelectableArmies { get; }
    }

    public class GameplayViewModel :
        Screen,
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
        private readonly IGameEngineClientProxy _gameEngineClientProxy;
        private string _informationText;
        private string _playerName;
        private Color _playerColor;
        private bool _canEnterFortifyMode;
        private bool _canEnterAttackMode;
        private bool _canEndTurn;
        private IInteractionState _interactionState;
        private IAttackPhase _attackPhase;
        private IList<PlayerStatusViewModel> _playerStatuses;
        private int _numberOfArmies;
        private int _maxNumberOfUserSelectableArmies;
        private Maybe<Region> _previouslySelectedAttackingRegion;
        private bool _canUserSelectNumberOfArmies;
        private IReadOnlyList<Services.GameEngineClient.Play.Territory> _territories;
        private IDisposable _draftArmiesSubscription;
        private IDisposable _attackSubscription;
        private IDisposable _sendArmiesSubscription;
        private IDisposable _endTurnSubscription;
        private IDisposable _gameOverSubscription;

        public GameplayViewModel(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IPlayerStatusViewModelFactory playerStatusViewModelFactory,
            IGameEngineClientProxy gameEngineClientProxy)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _playerStatusViewModelFactory = playerStatusViewModelFactory;
            _gameEngineClientProxy = gameEngineClientProxy;

            WorldMapViewModel = _worldMapViewModelFactory.Create(OnRegionClicked);
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText
        {
            get => _informationText;
            private set { this.NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        public string PlayerName
        {
            get => _playerName;
            private set { this.NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public Color PlayerColor
        {
            get => _playerColor;
            private set { this.NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x); }
        }

        public bool CanEnterFortifyMode
        {
            get => _canEnterFortifyMode;
            private set { this.NotifyOfPropertyChange(value, () => CanEnterFortifyMode, x => _canEnterFortifyMode = x); }
        }

        public bool CanEnterAttackMode
        {
            get => _canEnterAttackMode;
            private set { this.NotifyOfPropertyChange(value, () => CanEnterAttackMode, x => _canEnterAttackMode = x); }
        }

        public bool CanEndTurn
        {
            get => _canEndTurn;
            private set { this.NotifyOfPropertyChange(value, () => CanEndTurn, x => _canEndTurn = x); }
        }

        public IList<PlayerStatusViewModel> PlayerStatuses
        {
            get => _playerStatuses;
            private set { this.NotifyOfPropertyChange(value, () => PlayerStatuses, x => _playerStatuses = x); }
        }

        public int NumberOfUserSelectedArmies
        {
            get => _numberOfArmies;
            set => _numberOfArmies = value;
        }

        public int MaxNumberOfUserSelectableArmies
        {
            get => _maxNumberOfUserSelectableArmies;
            private set => this.NotifyOfPropertyChange(value, () => MaxNumberOfUserSelectableArmies, x => _maxNumberOfUserSelectableArmies = x);
        }

        public bool CanUserSelectNumberOfArmies
        {
            get => _canUserSelectNumberOfArmies;
            private set => this.NotifyOfPropertyChange(value, () => CanUserSelectNumberOfArmies, x => _canUserSelectNumberOfArmies = x);
        }

        public bool CanShowCards => true;

        protected override void OnActivate()
        {
            base.OnActivate();

            _draftArmiesSubscription = _gameEngineClientProxy.OnDraftArmies.Subscribe(DraftArmies);
            _attackSubscription = _gameEngineClientProxy.OnAttack.Subscribe(Attack);
            _sendArmiesSubscription = _gameEngineClientProxy.OnSendArmiesToOccupy.Subscribe(SendArmiesToOccupy);
            _endTurnSubscription = _gameEngineClientProxy.OnEndTurn.Subscribe(EndTurn);
            _gameOverSubscription = _gameEngineClientProxy.OnGameOver.Subscribe(GameOver);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);

            _draftArmiesSubscription.Dispose();
            _attackSubscription.Dispose();
            _sendArmiesSubscription.Dispose();
            _endTurnSubscription.Dispose();
            _gameOverSubscription.Dispose();
        }

        private void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            _territories = draftArmiesPhase.Territories;
            _previouslySelectedAttackingRegion = Maybe<Region>.Nothing;

            UpdatePlayersInformation(draftArmiesPhase.CurrentPlayerName, draftArmiesPhase.Players);

            ShowDraftArmiesView(draftArmiesPhase);
        }

        private void Attack(IAttackPhase attackPhase)
        {
            _territories = attackPhase.Territories;
            _attackPhase = attackPhase;

            UpdatePlayersInformation(attackPhase.CurrentPlayerName, attackPhase.Players);

            _previouslySelectedAttackingRegion.End(
                selectedRegion => ShowAttackPhaseView(attackPhase, selectedRegion),
                () => ShowAttackPhaseView(attackPhase));
        }

        private void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _territories = sendArmiesToOccupyPhase.Territories;

            UpdatePlayersInformation(sendArmiesToOccupyPhase.CurrentPlayerName, sendArmiesToOccupyPhase.Players);

            ShowSendArmiesToOccupyView(sendArmiesToOccupyPhase);
        }

        private void EndTurn(IEndTurnPhase endTurnPhase)
        {
            _territories = endTurnPhase.Territories;

            UpdatePlayersInformation(endTurnPhase.CurrentPlayerName, endTurnPhase.Players);

            ShowEndTurnView(endTurnPhase);
        }

        private void GameOver(IGameOverState gameOverState)
        {
            ShowGameOverMessage(gameOverState.Winner);
        }

        public void EnterAttackMode()
        {
            ShowAttackPhaseView(_attackPhase);
        }

        void ISelectAttackingRegionInteractionStateObserver.Select(Region selectedRegion)
        {
            _previouslySelectedAttackingRegion = Maybe<Region>.Create(selectedRegion);

            ShowAttackPhaseView(_attackPhase, selectedRegion);
        }

        void IAttackInteractionStateObserver.DeselectRegion()
        {
            ShowAttackPhaseView(_attackPhase);
        }

        public void ShowCards() {}

        public void EnterFortifyMode()
        {
            ShowFortifyView(_attackPhase);
        }

        void ISelectSourceRegionForFortificationInteractionStateObserver.Select(Region selectedRegion)
        {
            ShowFortifyView(_attackPhase, selectedRegion);
        }

        void IFortifyInteractionStateObserver.DeselectRegion()
        {
            ShowFortifyView(_attackPhase);
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

        private void OnRegionClicked(Region region)
        {
            _interactionState.OnRegionClicked(region);
        }

        private void ShowDraftArmiesView(IDraftArmiesPhase draftArmiesPhase)
        {
            _interactionState = _interactionStateFactory.CreateDraftArmiesInteractionState(draftArmiesPhase);

            UpdateView();
        }

        private void ShowAttackPhaseView(IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(attackPhase, this);

            UpdateView();
        }

        private void ShowAttackPhaseView(IAttackPhase attackPhase, Region selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateAttackInteractionState(attackPhase, selectedRegion, this);

            UpdateView();
        }

        private void ShowFortifyView(IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(attackPhase, this);

            UpdateView();
        }

        private void ShowFortifyView(IAttackPhase attackPhase, Region selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateFortifyInteractionState(attackPhase, selectedRegion, this);

            UpdateView();
        }

        private void ShowSendArmiesToOccupyView(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _interactionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);

            UpdateView();
        }

        private void ShowEndTurnView(IEndTurnPhase endTurnPhase)
        {
            _interactionState = _interactionStateFactory.CreateEndTurnInteractionState(endTurnPhase);

            UpdateView();
        }

        private void ShowGameOverMessage(string winner)
        {
            _dialogManager.ShowGameOverDialog(winner);

            _eventAggregator.PublishOnUIThread(new NewGameMessage());
        }

        private void UpdatePlayersInformation(string currentPlayerName, IReadOnlyList<Player> playerGameDatas)
        {
            PlayerName = currentPlayerName;
            PlayerColor = _playerUiDataRepository.Get(currentPlayerName).Color;

            PlayerStatuses = playerGameDatas
                .Select(x => _playerStatusViewModelFactory.Create(x))
                .ToList();
        }

        private void UpdateView()
        {
            InformationText = _interactionState.Title;
            CanEnterFortifyMode = _interactionState.CanEnterFortifyMode;
            CanEnterAttackMode = _interactionState.CanEnterAttackMode;
            CanEndTurn = _interactionState.CanEndTurn;
            CanUserSelectNumberOfArmies = _interactionState.CanUserSelectNumberOfArmies;
            NumberOfUserSelectedArmies = _interactionState.DefaultNumberOfUserSelectedArmies;
            MaxNumberOfUserSelectableArmies = _interactionState.MaxNumberOfUserSelectableArmies;

            _worldMapViewModelFactory.Update(
                WorldMapViewModel,
                Convert(_territories),
                _interactionState.SelectedRegion);
        }

        private IReadOnlyList<Territory> Convert(IEnumerable<Services.GameEngineClient.Play.Territory> territories)
        {
            return territories
                .Select(x => new Territory(x.Region, _interactionState.EnabledRegions.Contains(x.Region), x.PlayerName, x.Armies)).ToList();
        }
    }
}