using System;
using System.Threading.Tasks;
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

        [SetUp]
        public void SetUp()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameFactoryWorker = Substitute.For<IGameFactoryWorker>();
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _dispatcherWrapper = Substitute.For<IDispatcherWrapper>();

            _gameSetupViewModel = new GameSetupViewModel(_worldMapViewModelFactory, _gameFactoryWorker, _dispatcherWrapper, _gameStateConductor);
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
            var location = Substitute.For<ILocation>();

            ILocation actual = null;
            var workerTask = Task.Run(() => { actual = _gameSetupViewModel.GetLocationCallback(locationSelectorParameter); });
            _gameSetupViewModel.SelectLocation(location);
            var waited = workerTask.Wait(5000);

            waited.Should().BeTrue("worker task did not finish in time");
            actual.Should().Be(location);
        }

        [Test]
        public void Get_location_callback_updates_viewmodel()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            var worldMap = Substitute.For<IWorldMap>();
            locationSelectorParameter.WorldMap.Returns(worldMap);
            var location = Substitute.For<ILocation>();
            _gameSetupViewModel.MonitorEvents();
            DispatherRelaysAllActions();
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(worldMap, _gameSetupViewModel.SelectLocation).Returns(worldMapViewModel);

            var workerTask = Task.Run(() => { _gameSetupViewModel.GetLocationCallback(locationSelectorParameter); });
            _gameSetupViewModel.SelectLocation(location);
            var waited = workerTask.Wait(5000);

            waited.Should().BeTrue("worker task did not finish in time");
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
            _dispatcherWrapper.WhenForAnyArgs(x => x.Invoke(null)).Do(x => x.Arg<Action>().Invoke());
        }
    }
}