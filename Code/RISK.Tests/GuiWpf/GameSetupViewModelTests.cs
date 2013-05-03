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
        public void Select_location_gets_location()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            locationSelectorParameter.WorldMap.Returns(Substitute.For<IWorldMap>());
            var location = Substitute.For<ILocation>();

            var actual = _gameSetupViewModel.GetLocationCallback(locationSelectorParameter);

            actual.Should().Be(location);
        }
    }
}