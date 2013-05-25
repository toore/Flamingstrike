using System;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using NSubstitute.Experimental;
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
        private IGameSettingStateConductor _gameSettingStateConductor;
        private IUserInteractionSynchronizer _userInteractionSynchronizer;
        private IDialogManager _dialogManager;
        private IGameEventAggregator _gameEventAggregator;

        [SetUp]
        public void SetUp()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameFactoryWorker = Substitute.For<IGameFactoryWorker>();
            _gameSettingStateConductor = Substitute.For<IGameSettingStateConductor>();
            _userInteractionSynchronizer = Substitute.For<IUserInteractionSynchronizer>();
            _dialogManager = Substitute.For<IDialogManager>();
            _gameEventAggregator = Substitute.For<IGameEventAggregator>();

            var locationSelectorParameter = StubLocationSelectorParameter(null);

            _gameFactoryWorker.WhenForAnyArgs(x => x.BeginInvoke(null))
                .Do(x => x.Arg<IGameFactoryWorkerCallback>().GetLocationCallback(locationSelectorParameter));

            _gameSetupViewModel = new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _gameSettingStateConductor, _userInteractionSynchronizer, _dialogManager, _gameEventAggregator);

            _userInteractionSynchronizer.ClearReceivedCalls();
        }

        private ILocationSelectorParameter StubLocationSelectorParameter(Action<ILocation> selectLocation)
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();

            var worldMap = Substitute.For<IWorldMap>();
            locationSelectorParameter.WorldMap.Returns(worldMap);
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(worldMap, selectLocation).ReturnsForAnyArgs(worldMapViewModel);

            var player = Substitute.For<IPlayer>();
            locationSelectorParameter.PlayerDuringSetup = new PlayerDuringSetup(player, 0);

            return locationSelectorParameter;
        }

        [Test]
        public void Initialize_game_factory()
        {
            _gameFactoryWorker.Received().BeginInvoke(_gameSetupViewModel);
        }

        [Test]
        public void Get_location_callback_gets_location()
        {
            var locationSelectorParameter = StubLocationSelectorParameter(_gameSetupViewModel.SelectLocation);
            var location = WhenWaitForInputIsCalledInvokeSelectLocation();

            var actual = _gameSetupViewModel.GetLocationCallback(locationSelectorParameter);

            actual.Should().Be(location);
        }

        private ILocation WhenWaitForInputIsCalledInvokeSelectLocation()
        {
            var location = Substitute.For<ILocation>();

            _userInteractionSynchronizer.When(x => x.WaitForUserToBeDoneWithInteracting())
                .Do(x => _gameSetupViewModel.SelectLocation(location));

            return location;
        }

        [Test]
        public void Get_location_callback_waits_for_user_input()
        {
            _gameSetupViewModel.GetLocationCallback(null);

            Received.InOrder(() =>
                {
                    _userInteractionSynchronizer.RequestUserInteraction();
                    _userInteractionSynchronizer.WaitForUserToBeDoneWithInteracting();
                });
        }

        [Test]
        public void Select_location_makes_input_available_and_wait_for_new_input_request()
        {
            _gameSetupViewModel.SelectLocation(null);

            Received.InOrder(() =>
                {
                    _userInteractionSynchronizer.UserIsDoneInteracting();
                    _userInteractionSynchronizer.WaitForUserInteractionRequest();
                });
        }

        [Test]
        public void Select_location_updates_view_model()
        {
            _gameSetupViewModel.MonitorEvents();
            var parameter = StubLocationSelectorParameter(_gameSetupViewModel.SelectLocation);
            parameter.PlayerDuringSetup.Armies = 10;
            _gameSetupViewModel.GetLocationCallback(parameter);

            _gameSetupViewModel.SelectLocation(null);

            _gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
            _gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.Player);
            _gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Test]
        public void When_finished_input_is_requested()
        {
            _gameSetupViewModel.OnFinished(null);

            _userInteractionSynchronizer.Received().RequestUserInteraction();
        }

        [Test]
        public void When_finished_game_conductor_is_notified()
        {
            var game = Substitute.For<IGame>();

            _gameSetupViewModel.OnFinished(game);
            _gameSetupViewModel.SelectLocation(null);

            _gameSettingStateConductor.Received().StartGamePlay(game);
        }
    }
}