using System;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class GameSetupViewModelTests
    {
        private GameSetupViewModel _gameSetupViewModel;
        private IWorldMapViewModelFactory _worldMapViewModelFactory;
        private IGameFactoryWorker _gameFactoryWorker;
        private IGameStateConductor _gameStateConductor;
        private IDispatcherWrapper _dispatcherWrapper;
        private IUserInputRequest _userInputRequest;

        [SetUp]
        public void SetUp()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameFactoryWorker = Substitute.For<IGameFactoryWorker>();
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _dispatcherWrapper = Substitute.For<IDispatcherWrapper>();
            _userInputRequest = Substitute.For<IUserInputRequest>();

            _gameSetupViewModel = new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _dispatcherWrapper, _gameStateConductor, _userInputRequest);
        }

        [Test]
        public void Initialize_game_factory()
        {
            _gameFactoryWorker.Received().BeginInvoke(_gameSetupViewModel);
        }

        [Test]
        public void Get_location_callback_gets_location()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            locationSelectorParameter.WorldMap.Returns(Substitute.For<IWorldMap>());
            var location = WhenWaitForInputIsCalledInvokeSelectLocation();

            var actual = _gameSetupViewModel.GetLocationCallback(locationSelectorParameter);

            actual.Should().Be(location);
        }

        private ILocation WhenWaitForInputIsCalledInvokeSelectLocation()
        {
            var location = Substitute.For<ILocation>();

            _userInputRequest.When(x => x.WaitForInput())
                .Do(x => _gameSetupViewModel.SelectLocation(location));

            return location;
        }

        [Test]
        public void Get_location_callback_updates_viewmodel()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            var worldMap = Substitute.For<IWorldMap>();
            locationSelectorParameter.WorldMap.Returns(worldMap);
            DispatherRelaysAllActions();
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(worldMap, _gameSetupViewModel.SelectLocation).Returns(expectedWorldMapViewModel);
            WorldMapViewModel viewModelWhenWaitingForInput = null;
            _userInputRequest.When(x => x.WaitForInput())
                .Do(x => viewModelWhenWaitingForInput = _gameSetupViewModel.WorldMapViewModel);
            _gameSetupViewModel.MonitorEvents();

            _gameSetupViewModel.GetLocationCallback(locationSelectorParameter);
            _gameSetupViewModel.SelectLocation(Substitute.For<ILocation>());

            _gameSetupViewModel.WorldMapViewModel.Should().Be(viewModelWhenWaitingForInput);
            _gameSetupViewModel.WorldMapViewModel.Should().Be(expectedWorldMapViewModel);
            _gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
        }

        [Test]
        public void When_finished_game_conductor_is_notified_through_dispatcher()
        {
            var game = Substitute.For<IGame>();
            DispatherRelaysAllActions();

            _gameSetupViewModel.OnFinished(game);

            _gameStateConductor.Received().StartGamePlay(game);
        }

        private void DispatherRelaysAllActions()
        {
            _dispatcherWrapper
                .WhenForAnyArgs(x => x.Invoke(null))
                .Do(x => x.Arg<Action>().Invoke());
        }
    }
}