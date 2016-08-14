﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Messages;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay
{
    public class GameboardViewModel : ViewModelBase, IGameboardViewModel, IActivate
    {
        private readonly IRegions _regions;
        private readonly IGame _game;
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private string _playerName;
        private bool _canEnterFortifyMode;
        private bool _canEndTurn;

        public GameboardViewModel(
            IGame game,
            IInteractionStateFsm interactionStateFsm,
            IInteractionStateFactory interactionStateFactory,
            IRegions regions,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _regions = regions;
            _game = game;
            _interactionStateFsm = interactionStateFsm;
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;

            InformationText = ResourceManager.Instance.GetString("SELECT_TERRITORY");
        }

        public WorldMapViewModel WorldMapViewModel { get; private set; }

        public string PlayerName
        {
            get { return _playerName; }
            private set { NotifyOfPropertyChange(value, () => PlayerName, x => _playerName = x); }
        }

        public string InformationText { get; }

        public bool CanEnterFortifyMode
        {
            get { return _canEnterFortifyMode; }
            private set { NotifyOfPropertyChange(value, () => CanEnterFortifyMode, x => _canEnterFortifyMode = x); }
        }

        public bool CanEndTurn
        {
            get { return _canEndTurn; }
            private set { NotifyOfPropertyChange(value, () => CanEndTurn, x => _canEndTurn = x); }
        }

        public void Activate()
        {
            InitializeWorld();
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

        private void InitializeWorld()
        {
            var allTerritories = GetAllTerritories();
            WorldMapViewModel = _worldMapViewModelFactory.Create(allTerritories, OnRegionClick, Enumerable.Empty<IRegion>());

            EnterDraftArmiesInteractionState();

            UpdateGameboard();
        }

        private void EnterDraftArmiesInteractionState()
        {
            var draftArmiesState = _interactionStateFactory.CreateDraftArmiesInteractionState(_game);
            _interactionStateFsm.Set(draftArmiesState);
        }

        private ReadOnlyCollection<ITerritory> GetAllTerritories()
        {
            var territories = _regions.GetAll()
                .Select(region => _game.GetTerritory(region))
                .ToList().AsReadOnly();

            return territories;
        }

        public void EnterFortifyMode()
        {
            EnterFortifyInteractionState();
        }

        private void EnterFortifyInteractionState()
        {
            var fortifySelectState = _interactionStateFactory.CreateFortifySelectInteractionState(_game);
            _interactionStateFsm.Set(fortifySelectState);
        }

        public void EndTurn()
        {
            _game.EndTurn();

            EnterDraftArmiesInteractionState();

            UpdateGameboard();
        }

        public void EndGame()
        {
            var confirm = _dialogManager.ConfirmEndGame();

            if (confirm.HasValue && confirm.Value)
            {
                _eventAggregator.PublishOnUIThread(new NewGameMessage());
            }
        }

        public void OnRegionClick(IRegion region)
        {
            _interactionStateFsm.OnClick(region);

            UpdateGameboard();

            if (_game.IsGameOver())
            {
                ShowGameOverMessage();
            }
        }

        private void ShowGameOverMessage()
        {
            var gameOverViewModel = _gameOverViewModelFactory.Create(_game.CurrentPlayer.Name);
            _windowManager.ShowDialog(gameOverViewModel);
        }

        private void UpdateGameboard()
        {
            PlayerName = _game.CurrentPlayer.Name;

            CanEnterFortifyMode = _game.CanFreeMove();
            CanEndTurn = _game.CanEndTurn();

            UpdateWorldMap();
        }

        private void UpdateWorldMap()
        {
            var allTerritories = GetAllTerritories();
            var enabledTerritories = _regions.GetAll()
                .Where(x => _interactionStateFsm.CanClick(x))
                .ToList();

            var selectedRegion = _interactionStateFsm.SelectedRegion;

            _worldMapViewModelFactory.Update(WorldMapViewModel, allTerritories, selectedRegion, enabledTerritories);
        }
    }
}