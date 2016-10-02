using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Messages;
using RISK.Core;
using RISK.GameEngine.Play;
using Action = System.Action;

namespace GuiWpf.ViewModels.Gameplay
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
            InformationText = Resources.DRAFT_ARMIES;
            PlayerName = _game.CurrentPlayer.Name;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            CanEndTurn = false;

            _interactionState = _interactionStateFactory.CreateDraftArmiesInteractionState(draftArmiesPhase);

            UpdateWorldMap(draftArmiesPhase);
        }

        private void UpdateWorldMap(IDraftArmiesPhase draftArmiesPhase)
        {
            UpdateWorldMap(_game.Territories, draftArmiesPhase.RegionsAllowedToDraftArmies, null);
        }

        public void Attack(IAttackPhase attackPhase)
        {
            _attackPhase = attackPhase;

            InformationText = Resources.ATTACK;
            PlayerName = _game.CurrentPlayer.Name;

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;
            CanEndTurn = true;
            _endTurn = attackPhase.EndTurn;

            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            UpdateWorldMap(attackPhase);
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            InformationText = Resources.SEND_ARMIES_TO_OCCUPY;
            PlayerName = _game.CurrentPlayer.Name;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = false;
            CanEndTurn = false;

            _interactionState = _interactionStateFactory.CreateSendArmiesToOccupyInteractionState(sendArmiesToOccupyPhase);

            UpdateWorldMap(sendArmiesToOccupyPhase);
        }

        private void UpdateWorldMap(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            UpdateWorldMap(
                _game.Territories,
                new[] { sendArmiesToOccupyPhase.OccupiedRegion },
                sendArmiesToOccupyPhase.AttackingRegion);
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            InformationText = Resources.END_TURN;
            PlayerName = _game.CurrentPlayer.Name;

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;
            CanEndTurn = true;
            _endTurn = endTurnPhase.EndTurn;

            UpdateWorldMapInEndOfTurn();
        }

        private void UpdateWorldMapInEndOfTurn()
        {
            UpdateWorldMap(_game.Territories, new IRegion[] { }, null);
        }

        public void GameOver(IGameIsOver gameIsOver)
        {
            ShowGameOverMessage();
        }

        void ISelectAttackingRegionObserver.SelectRegionToAttackFrom(IRegion region)
        {
            _interactionState = _interactionStateFactory.CreateAttackInteractionState(_attackPhase, region, this);

            CanEnterFortifyMode = false;

            var regionsThatCanBeInteractedWith = _attackPhase.GetRegionsThatCanBeAttacked(region)
                .Concat(new[] { region }).ToList();

            UpdateWorldMap(
                _game.Territories,
                regionsThatCanBeInteractedWith,
                region);
        }

        void ISelectSourceRegionForFortificationObserver.SelectSourceRegion(IRegion region)
        {
            _interactionState = _interactionStateFactory.CreateFortifyInteractionState(_attackPhase, region, this);

            CanEnterAttackMode = false;

            var regionsThatCanBeInteractedWith = _attackPhase.GetRegionsThatCanBeFortified(region)
                .Concat(new[] { region }).ToList();

            UpdateWorldMap(
                _game.Territories,
                regionsThatCanBeInteractedWith,
                region);
        }

        void IDeselectAttackingRegionObserver.DeselectRegion()
        {
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            CanEnterFortifyMode = true;

            UpdateWorldMap(_attackPhase);
        }

        void IDeselectRegionToFortifyFromObserver.DeselectRegion()
        {
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(this);

            CanEnterAttackMode = true;

            UpdateWorldMap(_attackPhase);
        }

        private void UpdateWorldMap(IAttackPhase attackPhase)
        {
            UpdateWorldMap(_game.Territories, attackPhase.RegionsThatCanBeSourceForAttackOrFortification, selectedRegion: null);
        }

        private void UpdateWorldMap(IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> enabledRegions, IRegion selectedRegion)
        {
            _worldMapViewModelFactory.Update(WorldMapViewModel, territories, enabledRegions, selectedRegion);
        }

        public void EnterFortifyMode()
        {
            _interactionState = _interactionStateFactory.CreateSelectSourceRegionForFortificationInteractionState(this);

            InformationText = Resources.FORTIFY;

            CanEnterFortifyMode = false;
            CanEnterAttackMode = true;
        }

        public void EnterAttackMode()
        {
            _interactionState = _interactionStateFactory.CreateSelectAttackingRegionInteractionState(this);

            InformationText = Resources.ATTACK;

            CanEnterFortifyMode = true;
            CanEnterAttackMode = false;
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
            var gameOverViewModel = _gameOverViewModelFactory.Create(_game.CurrentPlayer.Name);
            _windowManager.ShowDialog(gameOverViewModel);
        }
    }
}