using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay.Map;
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
        private IGameFactory _gameFactory;
        private IGameStateConductor _gameStateConductor;

        [SetUp]
        public void SetUp()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameFactory = Substitute.For<IGameFactory>();
            _gameStateConductor = Substitute.For<IGameStateConductor>();

            _gameSetupViewModel = new GameSetupViewModel(_worldMapViewModelFactory, _gameFactory, _gameStateConductor);
        }

        [Test]
        [Ignore]
        public void Select_location_gets_location()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            locationSelectorParameter.WorldMap.Returns(Substitute.For<IWorldMap>());
            var location = Substitute.For<ILocation>();

            var actual = _gameSetupViewModel.GetLocation(locationSelectorParameter);

            actual.Should().Be(location);
        }
    }
}