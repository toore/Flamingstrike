using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class TurnFactoryTests
    {
        private TurnFactory _factory;
        private IBattleCalculator _battleCalculator;
        private ICardFactory _cardFactory;

        [SetUp]
        public void SetUp()
        {
            _battleCalculator = Substitute.For<IBattleCalculator>();
            _cardFactory = Substitute.For<ICardFactory>();

            _factory = new TurnFactory(_battleCalculator, _cardFactory);
        }

        [Test]
        public void Create_initializes_turn()
        {
            var player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();

            var turn = _factory.Create(player, worldMap);

            turn.Should().NotBeNull();
        }
    }
}