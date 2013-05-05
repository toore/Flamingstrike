using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay.Map;
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
        private IGameStateConductor _gameStateConductor;
        private IInputRequestHandler _inputRequestHandler;

        [SetUp]
        public void SetUp()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameFactoryWorker = Substitute.For<IGameFactoryWorker>();
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _inputRequestHandler = Substitute.For<IInputRequestHandler>();

            var locationSelectorParameter = StubLocationSelectorParameter();

            _gameFactoryWorker.WhenForAnyArgs(x => x.BeginInvoke(null))
                .Do(x => x.Arg<IGameFactoryWorkerCallback>().GetLocationCallback(locationSelectorParameter));

            _gameSetupViewModel = new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _gameStateConductor, _inputRequestHandler);

            _inputRequestHandler.ClearReceivedCalls();
        }

        private ILocationSelectorParameter StubLocationSelectorParameter()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            var worldMap = Substitute.For<IWorldMap>();
            locationSelectorParameter.WorldMap.Returns(worldMap);
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(worldMap, null).ReturnsForAnyArgs(expectedWorldMapViewModel);

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
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            locationSelectorParameter.WorldMap.Returns(Substitute.For<IWorldMap>());
            var location = WhenWaitForInputIsCalledInvokeSelectLocation();

            var actual = _gameSetupViewModel.GetLocationCallback(locationSelectorParameter);

            actual.Should().Be(location);
        }

        private ILocation WhenWaitForInputIsCalledInvokeSelectLocation()
        {
            var location = Substitute.For<ILocation>();

            _inputRequestHandler.When(x => x.WaitForInputAvailable())
                .Do(x => _gameSetupViewModel.SelectLocation(location));

            return location;
        }

        [Test]
        public void Get_location_callback_updates_viewmodel_waits_for_user_input()
        {
            _gameSetupViewModel.GetLocationCallback(null);

            Received.InOrder(() =>
                {
                    _inputRequestHandler.RequestInput();
                    _inputRequestHandler.WaitForInputAvailable();
                });
        }

        [Test]
        public void Select_location_makes_input_available_and_wait_for_new_input_request()
        {
            _gameSetupViewModel.SelectLocation(null);

            Received.InOrder(() =>
                {
                    _inputRequestHandler.InputIsAvailable();
                    _inputRequestHandler.WaitForInputRequest();
                });
        }

        [Test]
        public void When_finished_input_is_requested()
        {
            _gameSetupViewModel.OnFinished(null);

            _inputRequestHandler.Received().RequestInput();
        }

        [Test]
        public void When_finished_game_conductor_is_notified()
        {
            var game = Substitute.For<IGame>();

            _gameSetupViewModel.OnFinished(game);
            _gameSetupViewModel.SelectLocation(null);

            _gameStateConductor.Received().StartGamePlay(game);
        }
    }
}