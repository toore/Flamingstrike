using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Caliburn.Micro;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using RISK.UI.WPF.ViewModels.Messages;
using RISK.UI.WPF.ViewModels.Preparation;
using Action = System.Action;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IGameplayViewModel : IMainViewModel, IGameObserver {}

    public class GameplayViewModel :
        ViewModelBase,
        IGameplayViewModel,
        IActivate,
        ISelectAttackingRegionInteractionStateObserver,
        IAttackInteractionStateObserver,
        ISelectFortificationInteractionStateObserver,
        IFortifyInteractionStateObserver
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private string _informationText;
        private string _playerName;
        private Color _playerColor;
        private bool _canEnterFortifyMode;
        private bool _canEnterAttackMode;
        private bool _canEndTurn;
        private IInteractionState _interactionState;
        private IAttackPhase _attackPhase;
        private Action _endTurnAction;
        private IList<PlayerAndNumberOfCardsViewModel> _players;
        private Maybe<IRegion> _previouslySelectedAttackingRegion;

        public GameplayViewModel(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IPlayerUiDataRepository playerUiDataRepository,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _playerUiDataRepository = playerUiDataRepository;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            WorldMapViewModel = _worldMapViewModelFactory.Create(OnRegionClick);
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string InformationText
        {
            get { return _informationText; }
            private set { NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        public string PlayerName
        {
            get { return _playerName; }
            private set { NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public Color PlayerColor
        {
            get { return _playerColor; }
            private set { NotifyOfPropertyChange(value, () => PlayerColor, x => _playerColor = x); }
        }

        public bool CanEnterFortifyMode
        {
            get { return _canEnterFortifyMode; }
            private set { NotifyOfPropertyChange(value, () => CanEnterFortifyMode, x => _canEnterFortifyMode = x); }
        }

        public bool CanEnterAttackMode
        {
            get { return _canEnterAttackMode; }
            private set { NotifyOfPropertyChange(value, () => CanEnterAttackMode, x => _canEnterAttackMode = x); }
        }

        public bool CanEndTurn
        {
            get { return _canEndTurn; }
            private set { NotifyOfPropertyChange(value, () => CanEndTurn, x => _canEndTurn = x); }
        }

        public bool IsActive
        {
            get { throw new InvalidOperationException($"{nameof(IsActive)} is not used"); }
        }

        public event EventHandler<ActivationEventArgs> Activated
        {
            add { throw new InvalidOperationException($"{nameof(Activated)} is not used"); }
            remove { throw new InvalidOperationException($"{nameof(Activated)} is not used"); }
        }

        public IList<PlayerAndNumberOfCardsViewModel> Players
        {
            get { return _players; }
            private set { NotifyOfPropertyChange(value, () => Players, x => _players = x); }
        }

        public void Activate() {}

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            _previouslySelectedAttackingRegion = Maybe<IRegion>.Nothing;
            UpdatePlayersInformation(draftArmiesPhase);

            ShowDraftArmiesView(draftArmiesPhase);
        }

        public void Attack(IAttackPhase attackPhase)
        {
            _attackPhase = attackPhase;
            SetEndTurnAction(Maybe<Action>.Create(attackPhase.EndTurn));

            UpdatePlayersInformation(attackPhase);

            _previouslySelectedAttackingRegion.End(
                selectedRegion => ShowAttackPhaseView(attackPhase, selectedRegion),
                () => ShowAttackPhaseView(attackPhase));
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            UpdatePlayersInformation(sendArmiesToOccupyPhase);

            ShowSendArmiesToOccupyView(sendArmiesToOccupyPhase);
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            UpdatePlayersInformation(endTurnPhase);

            ShowEndTurnView(endTurnPhase);
        }

        public void GameOver(IGameOverState gameOverState)
        {
            ShowGameOverMessage(gameOverState.Winner);
        }

        public void EnterAttackMode()
        {
            ShowAttackPhaseView(_attackPhase);
        }

        void ISelectAttackingRegionInteractionStateObserver.Select(IRegion selectedRegion)
        {
            _previouslySelectedAttackingRegion = Maybe<IRegion>.Create(selectedRegion);

            ShowAttackPhaseView(_attackPhase, selectedRegion);
        }

        void IAttackInteractionStateObserver.DeselectRegion()
        {
            ShowAttackPhaseView(_attackPhase);
        }

        public void EnterFortifyMode()
        {
            ShowFortifyView(_attackPhase);
        }

        void ISelectFortificationInteractionStateObserver.Select(IRegion selectedRegion)
        {
            ShowFortifyView(_attackPhase, selectedRegion);
        }

        void IFortifyInteractionStateObserver.DeselectRegion()
        {
            ShowFortifyView(_attackPhase);
        }

        public void EndTurn()
        {
            _endTurnAction();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        private void OnRegionClick(IRegion region)
        {
            _interactionState.OnClick(region);
        }

        private void ShowDraftArmiesView(IDraftArmiesPhase draftArmiesPhase)
        {
            _interactionState = _interactionStateFactory.CreateDraftArmiesInteractionState(draftArmiesPhase);

            InformationText = string.Format(Resources.DRAFT_ARMIES, draftArmiesPhase.NumberOfArmiesToDraft);

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            SetEndTurnAction(Maybe<Action>.Nothing);

            UpdateWorldMap(draftArmiesPhase);
        }

        private void ShowAttackPhaseView(IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            InformationText = Resources.ATTACK_SELECT_FROM_TERRITORY;

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;

            UpdateWorldMap(attackPhase);
        }

        private void ShowAttackPhaseView(IAttackPhase attackPhase, IRegion selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateAttackInteractionState(attackPhase, selectedRegion, this);

            InformationText = Resources.ATTACK_SELECT_TERRITORY_TO_ATTACK;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;

            var regionsThatCanBeInteractedWith = attackPhase.GetRegionsThatCanBeAttacked(selectedRegion)
                .Concat(new[] { selectedRegion }).ToList();

            UpdateWorldMap(
                attackPhase.Territories,
                regionsThatCanBeInteractedWith,
                Maybe<IRegion>.Create(selectedRegion));
        }

        private void ShowFortifyView(IAttackPhase attackPhase)
        {
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(this);

            InformationText = Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_FROM;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = true;

            UpdateWorldMap(attackPhase);
        }

        private void ShowFortifyView(IAttackPhase attackPhase, IRegion selectedRegion)
        {
            _interactionState = _interactionStateFactory.CreateFortifyInteractionState(attackPhase, selectedRegion, this);

            InformationText = Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_TO;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;

            var regionsThatCanBeInteractedWith = attackPhase.GetRegionsThatCanBeFortified(selectedRegion)
                .Concat(new[] { selectedRegion }).ToList();

            UpdateWorldMap(
                attackPhase.Territories,
                regionsThatCanBeInteractedWith,
                Maybe<IRegion>.Create(selectedRegion));
        }

        private void ShowSendArmiesToOccupyView(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _interactionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);

            InformationText = Resources.SEND_ARMIES_TO_OCCUPY;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            SetEndTurnAction(Maybe<Action>.Nothing);

            UpdateWorldMap(sendArmiesToOccupyPhase);
        }

        private void ShowEndTurnView(IEndTurnPhase endTurnPhase)
        {
            InformationText = Resources.END_TURN;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            SetEndTurnAction(Maybe<Action>.Create(endTurnPhase.EndTurn));

            UpdateWorldMap(endTurnPhase);
        }

        private void ShowGameOverMessage(IPlayer winner)
        {
            _dialogManager.ShowGameOverDialog(winner.Name);

            _eventAggregator.PublishOnUIThread(new NewGameMessage());
        }

        private void SetEndTurnAction(Maybe<Action> endTurnAction)
        {
            Action noAction = () => { };
            _endTurnAction = endTurnAction.Fold(x => x, () => noAction);
            CanEndTurn = endTurnAction.Fold(x => true, () => false);
        }

        private void UpdatePlayersInformation(IGameStatus gameStatus)
        {
            var currentPlayer = gameStatus.Player;
            PlayerName = currentPlayer.Name;
            PlayerColor = _playerUiDataRepository.Get(currentPlayer).Color;

            Players = gameStatus.PlayerGameDatas
                .Select(CreatePlayerAndNumberOfCardsViewModel)
                .ToList();
        }

        private PlayerAndNumberOfCardsViewModel CreatePlayerAndNumberOfCardsViewModel(IPlayerGameData playerGameData)
        {
            var player = playerGameData.Player;
            var playerUiData = _playerUiDataRepository.Get(player);

            return new PlayerAndNumberOfCardsViewModel(player.Name, playerUiData.Color, playerGameData.Cards.Count);
        }

        private void UpdateWorldMap(IDraftArmiesPhase draftArmiesPhase)
        {
            UpdateWorldMap(draftArmiesPhase.Territories, draftArmiesPhase.RegionsAllowedToDraftArmies, Maybe<IRegion>.Nothing);
        }

        private void UpdateWorldMap(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            UpdateWorldMap(
                sendArmiesToOccupyPhase.Territories,
                new[] { sendArmiesToOccupyPhase.OccupiedRegion },
                Maybe<IRegion>.Create(sendArmiesToOccupyPhase.AttackingRegion));
        }

        private void UpdateWorldMap(IEndTurnPhase endTurnPhase)
        {
            UpdateWorldMap(endTurnPhase.Territories, new IRegion[] { }, Maybe<IRegion>.Nothing);
        }

        private void UpdateWorldMap(IAttackPhase attackPhase)
        {
            UpdateWorldMap(attackPhase.Territories, attackPhase.RegionsThatCanBeSourceForAttackOrFortification, Maybe<IRegion>.Nothing);
        }

        private void UpdateWorldMap(IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, Maybe<IRegion> selectedRegion)
        {
            _worldMapViewModelFactory.Update(WorldMapViewModel, territories, enabledRegions, selectedRegion);
        }
    }
}