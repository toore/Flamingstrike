﻿using System.Linq;
using Caliburn.Micro;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public class GameSetupViewModel : ViewModelBase, IGameSetupViewModel
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IInputRequestHandler _inputRequestHandler;
        private readonly IUserNotifier _userNotifier;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;
        private ILocation _selectedLocation;
        private ILocationSelectorParameter _locationSelectorParameter;
        private bool _isGameSetupFinished;
        private IGame _game;

        public GameSetupViewModel(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IGameFactoryWorker gameFactoryWorker,
            IGameStateConductor gameStateConductor,
            IInputRequestHandler inputRequestHandler,
            IUserNotifier userNotifier,
            IResourceManagerWrapper resourceManagerWrapper)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _gameFactoryWorker = gameFactoryWorker;
            _gameStateConductor = gameStateConductor;
            _inputRequestHandler = inputRequestHandler;
            _userNotifier = userNotifier;
            _resourceManagerWrapper = resourceManagerWrapper;

            _gameFactoryWorker.BeginInvoke(this);

            WaitForUserInputRequestAndUpdateView();
        }

        private WorldMapViewModel _worldMapViewModel;
        public WorldMapViewModel WorldMapViewModel
        {
            get { return _worldMapViewModel; }
            private set { NotifyOfPropertyChange(value, () => WorldMapViewModel, x => _worldMapViewModel = x); }
        }

        private string _informationText;
        public string InformationText
        {
            get { return _informationText; }
            set { NotifyOfPropertyChange(value, () => InformationText, x => _informationText = x); }
        }

        private IPlayer _player;
        public IPlayer Player
        {
            get { return _player; }
            private set { NotifyOfPropertyChange(value, () => Player, x => _player = x); }
        }

        private void WaitForUserInputRequestAndUpdateView()
        {
            _inputRequestHandler.WaitForInputRequest();

            UpdateView(_locationSelectorParameter);
        }

        public ILocation GetLocationCallback(ILocationSelectorParameter locationSelectorParameter)
        {
            _locationSelectorParameter = locationSelectorParameter;

            _inputRequestHandler.RequestInput();
            _inputRequestHandler.WaitForInputAvailable();

            return _selectedLocation;
        }

        public void SelectLocation(ILocation location)
        {
            _selectedLocation = location;

            _inputRequestHandler.InputIsAvailable();
            WaitForUserInputRequestAndUpdateView();

            if (_isGameSetupFinished)
            {
                StartGamePlay(_game);
            }
        }

        public void OnFinished(IGame game)
        {
            _isGameSetupFinished = true;
            _game = game;

            _inputRequestHandler.RequestInput();
        }

        private void UpdateView(ILocationSelectorParameter locationSelectorParameter)
        {
            var worldMapViewModel = _worldMapViewModelFactory.Create(locationSelectorParameter.WorldMap, SelectLocation);

            worldMapViewModel.WorldMapViewModels.OfType<TerritoryLayoutViewModel>()
                .Apply(x => x.IsEnabled = locationSelectorParameter.AvailableLocations.Contains(x.Location));

            WorldMapViewModel = worldMapViewModel;

            Player = _locationSelectorParameter.PlayerDuringSetup.GetInGamePlayer();

            InformationText = string.Format(Resources.PLACE_ARMY, locationSelectorParameter.PlayerDuringSetup.Armies);
        }

        private void StartGamePlay(IGame game)
        {
            _gameStateConductor.StartGamePlay(game);
        }

        public bool CanFortify()
        {
            return false;
        }

        public void Fortify() {}

        public bool CanEndTurn()
        {
            return false;
        }

        public void EndTurn() {}

        public void EndGame()
        {
            var confirm = _userNotifier.Confirm(_resourceManagerWrapper.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME"));
        }
    }
}