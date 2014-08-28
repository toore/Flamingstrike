using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnFactoryTests
    {
        private TurnFactory _factory;
        private IBattleCalculator _battleCalculator;
        private ICardFactory _cardFactory;

        public TurnFactoryTests()
        {
            _battleCalculator = Substitute.For<IBattleCalculator>();
            _cardFactory = Substitute.For<ICardFactory>();

            _factory = new TurnFactory(_battleCalculator, _cardFactory);
        }

        [Fact]
        public void Create_initializes_turn()
        {
            var player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();

            var turn = _factory.Create(player, worldMap);

            turn.Should().NotBeNull();
        }
    }
}