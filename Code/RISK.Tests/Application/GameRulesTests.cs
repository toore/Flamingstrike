using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public abstract class GameRulesTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _otherPlayer;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _borderingRegion;
        private readonly GameRules _sut;

        protected GameRulesTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingRegion = Substitute.For<IRegion>();
            _borderingRegion = Substitute.For<IRegion>();

            _sut = new GameRules();
        }

        public class GameRulesBorderingTerritoriesTests : GameRulesTests
        {
            private readonly IRegion _anotherBorderingRegion;

            public GameRulesBorderingTerritoriesTests()
            {
                _anotherBorderingRegion = Substitute.For<IRegion>();

                _attackingRegion.HasBorder(_borderingRegion).Returns(true);
                _attackingRegion.HasBorder(_anotherBorderingRegion).Returns(true);
            }

            [Fact]
            public void Has_attackee_candidate()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_borderingRegion, _otherPlayer, 1),
                };

                var actual = _sut.GetCandidatesToAttack(_attackingRegion, territories);

                actual.Should().BeEquivalentTo(_borderingRegion);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 1),
                    new Territory(_borderingRegion, _otherPlayer, 1),
                };

                var actual = _sut.GetCandidatesToAttack(_attackingRegion, territories);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_borderingRegion, _otherPlayer, 1),
                    new Territory(_anotherBorderingRegion, _otherPlayer, 1)
                };

                var actual = _sut.GetCandidatesToAttack(_attackingRegion, territories);

                actual.Should().BeEquivalentTo(_borderingRegion, _anotherBorderingRegion);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_borderingRegion, _currentPlayer, 1)
                };

                var actual = _sut.GetCandidatesToAttack(_attackingRegion, territories);

                actual.Should().BeEmpty();
            }
        }

        public class GameRulesOrphanedTerritoriesTests : GameRulesTests
        {
            [Fact]
            public void Can_not_attack()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_borderingRegion, _otherPlayer, 1)
                };

                var actual = _sut.GetCandidatesToAttack(_attackingRegion, territories);

                actual.Should().BeEmpty();
            }
        }
    }
}