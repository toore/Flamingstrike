using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class AlternateGameSetupTests
    {
        private AlternateGameSetup _alternateGameSetup;
        private IWorldMap _worldMap;
        private IRandomizedPlayerRepository _randomizedPlayerRepository;
        private ILocationRepository _locationRepository;
        private ILocation _location1;
        private ILocation _location2;
        private IPlayer _player1;
        private IPlayer _player2;
        private ITerritory _territory1;
        private ITerritory _territory2;
        private IRandomWrapper _randomWrapper;

        [SetUp]
        public void SetUp()
        {
            _randomizedPlayerRepository = MockRepository.GenerateStub<IRandomizedPlayerRepository>();
            _locationRepository = MockRepository.GenerateStub<ILocationRepository>();
            _randomWrapper = MockRepository.GenerateStub<IRandomWrapper>();

            _player1 = MockRepository.GenerateStub<IPlayer>();
            _player2 = MockRepository.GenerateStub<IPlayer>();
            _randomizedPlayerRepository
                .Stub(x => x.GetAllInRandomizedOrder())
                .Return(new[] { _player1, _player2 });

            _location1 = MockRepository.GenerateStub<ILocation>();
            _location2 = MockRepository.GenerateStub<ILocation>();
            _locationRepository
                .Stub(x => x.GetAll())
                .Return(new[] { _location1, _location2 });

            _alternateGameSetup = new AlternateGameSetup(_randomizedPlayerRepository, _locationRepository, _randomWrapper);

            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _territory1 = MockRepository.GenerateStub<ITerritory>();
            _territory2 = MockRepository.GenerateStub<ITerritory>();
            _worldMap.Stub(x => x.GetTerritory(_location1)).Return(_territory1);
            _worldMap.Stub(x => x.GetTerritory(_location2)).Return(_territory2);
        }

        [Test]
        public void Initializes_all_territories_with_owners()
        {
            _alternateGameSetup.Initialize(_worldMap);

            _worldMap.GetTerritory(_location1).HasOwner().Should().BeTrue();
            _worldMap.GetTerritory(_location2).HasOwner().Should().BeTrue();
        }
    }
}