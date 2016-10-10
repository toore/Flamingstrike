using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using RISK.UI.WPF.ViewModels.Messages;
using Action = System.Action;

namespace RISK.UI.WPF.ViewModels.Gameplay
{
    public interface IGameplayViewModel : IMainViewModel, IGameObserver {}

    public class GameplayViewModel :
        ViewModelBase,
        IGameplayViewModel,
        IActivate,
        ISelectAttackingRegionObserver,
        IDeselectAttackingRegionObserver,
        ISelectSourceRegionForFortificationObserver,
        IDeselectRegionToFortifyFromObserver
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private string _playerName;
        private bool _canEnterFortifyMode;
        private bool _canEnterAttackMode;
        private bool _canEndTurn;
        private IInteractionState _interactionState;
        private string _informationText;
        private IGame _game;
        private IAttackPhase _attackPhase;
        private Action _endTurn;

        public GameplayViewModel(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            WorldMapViewModel = _worldMapViewModelFactory.Create(OnRegionClick);
        }

        public WorldMapViewModel WorldMapViewModel { get; }

        public string PlayerName
        {
            get { return _playerName; }
            private set { NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public string InformationText
        {
            get { return _informationText; }
            private set { NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
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

        public void Activate() {}

        private void OnRegionClick(IRegion region)
        {
            _interactionState.OnClick(region);
        }

        public void NewGame(IGame game)
        {
            _game = game;
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            PlayerName = _game.CurrentPlayerGameData.Player.Name;

            InformationText = string.Format(Resources.DRAFT_ARMIES, draftArmiesPhase.NumberOfArmiesToDraft);
            _interactionState = _interactionStateFactory.CreateDraftArmiesInteractionState(draftArmiesPhase);

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            CanEndTurn = false;

            UpdateWorldMap(draftArmiesPhase);
        }

        public void Attack(IAttackPhase attackPhase)
        {
            PlayerName = _game.CurrentPlayerGameData.Player.Name;

            _attackPhase = attackPhase;
            _endTurn = attackPhase.EndTurn;
            CanEndTurn = true;

            InformationText = Resources.ATTACK_SELECT_FROM_TERRITORY;
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;

            UpdateWorldMap(attackPhase);
        }

        public void EnterAttackMode()
        {
            InformationText = Resources.ATTACK_SELECT_FROM_TERRITORY;
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;
        }

        void ISelectAttackingRegionObserver.SelectRegionToAttackFrom(IRegion region)
        {
            InformationText = Resources.ATTACK_SELECT_TERRITORY_TO_ATTACK;
            _interactionState = _interactionStateFactory.CreateAttackInteractionState(_attackPhase, region, this);

            CanEnterFortifyMode = false;

            var regionsThatCanBeInteractedWith = _attackPhase.GetRegionsThatCanBeAttacked(region)
                .Concat(new[] { region }).ToList();

            UpdateWorldMap(
                _game.Territories,
                regionsThatCanBeInteractedWith,
                region);
        }

        void IDeselectAttackingRegionObserver.DeselectRegion()
        {
            InformationText = Resources.ATTACK_SELECT_FROM_TERRITORY;
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            CanEnterFortifyMode = true;

            UpdateWorldMap(_attackPhase);
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            PlayerName = _game.CurrentPlayerGameData.Player.Name;
            InformationText = Resources.SEND_ARMIES_TO_OCCUPY;
            _interactionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            CanEndTurn = false;

            UpdateWorldMap(sendArmiesToOccupyPhase);
        }

        public void EnterFortifyMode()
        {
            InformationText = Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_FROM;
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(this);

            CanEnterFortifyMode = false;
            CanEnterAttackMode = true;
        }

        void ISelectSourceRegionForFortificationObserver.SelectSourceRegion(IRegion region)
        {
            InformationText = Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_TO;
            _interactionState = _interactionStateFactory.CreateFortifyInteractionState(_attackPhase, region, this);

            CanEnterAttackMode = false;

            var regionsThatCanBeInteractedWith = _attackPhase.GetRegionsThatCanBeFortified(region)
                .Concat(new[] { region }).ToList();

            UpdateWorldMap(
                _game.Territories,
                regionsThatCanBeInteractedWith,
                region);
        }

        void IDeselectRegionToFortifyFromObserver.DeselectRegion()
        {
            InformationText = Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_FROM;
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(this);

            CanEnterAttackMode = true;

            UpdateWorldMap(_attackPhase);
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            PlayerName = _game.CurrentPlayerGameData.Player.Name;
            InformationText = Resources.END_TURN;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            CanEndTurn = true;
            _endTurn = endTurnPhase.EndTurn;

            UpdateWorldMapInEndOfTurn();
        }

        public void GameOver(IGameIsOver gameIsOver)
        {
            ShowGameOverMessage();
        }

        private void UpdateWorldMap(IDraftArmiesPhase draftArmiesPhase)
        {
            UpdateWorldMap(_game.Territories, draftArmiesPhase.RegionsAllowedToDraftArmies, null);
        }

        private void UpdateWorldMap(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            UpdateWorldMap(
                _game.Territories,
                new[] { sendArmiesToOccupyPhase.OccupiedRegion },
                sendArmiesToOccupyPhase.AttackingRegion);
        }

        private void UpdateWorldMapInEndOfTurn()
        {
            UpdateWorldMap(_game.Territories, new IRegion[] { }, null);
        }

        private void UpdateWorldMap(IAttackPhase attackPhase)
        {
            UpdateWorldMap(_game.Territories, attackPhase.RegionsThatCanBeSourceForAttackOrFortification, selectedRegion: null);
        }

        private void UpdateWorldMap(IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, IRegion selectedRegion)
        {
            _worldMapViewModelFactory.Update(WorldMapViewModel, territories, enabledRegions, selectedRegion);
        }

        public void EndTurn()
        {
            _endTurn();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        private void ShowGameOverMessage()
        {
            var gameOverViewModel = _gameOverViewModelFactory.Create(_game.CurrentPlayerGameData.Player.Name);
            _windowManager.ShowDialog(gameOverViewModel);
        }
    }
}