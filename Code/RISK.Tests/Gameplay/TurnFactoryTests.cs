using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.GamePlaying;
using Rhino.Mocks;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class TurnFactoryTests
    {
        private TurnFactory _factory;
        private IWorldMap _worldMap;
        private IBattleCalculator _battleCalculator;

        [SetUp]
        public void SetUp()
        {
            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _battleCalculator = MockRepository.GenerateStub<IBattleCalculator>();

            _factory = new TurnFactory(_worldMap, _battleCalculator);
        }

        [Test]
        public void Create_initializes_turn()
        {
            var turn = _factory.Create(null);

            turn.Should().NotBeNull();
        }
    }
}