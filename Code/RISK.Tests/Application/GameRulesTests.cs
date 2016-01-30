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
        private readonly IRegion _attackeeRegion;
        private readonly GameRules _sut;

        protected GameRulesTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingRegion = Substitute.For<IRegion>();
            _attackeeRegion = Substitute.For<IRegion>();

            _sut = new GameRules();
        }

        public class GameRulesBorderingTerritoriesTests : GameRulesTests
        {
            private readonly IRegion _secondAttackeeRegion;

            public GameRulesBorderingTerritoriesTests()
            {
                _secondAttackeeRegion = Substitute.For<IRegion>();

                _attackingRegion.HasBorder(_attackeeRegion).Returns(true);
                _attackingRegion.HasBorder(_secondAttackeeRegion).Returns(true);
            }

            [Fact]
            public void Has_attackee_candidate()
            {
                var territories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_attackeeRegion, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingRegion, territories);

                actual.Should().BeEquivalentTo(_attackeeRegion);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 1),
                    new Territory(_attackeeRegion, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingRegion, gameboardTerritories);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_attackeeRegion, _otherPlayer, 1),
                    new Territory(_secondAttackeeRegion, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingRegion, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeRegion, _secondAttackeeRegion);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_attackeeRegion, _currentPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingRegion, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }

        public class GameRulesOrphanedTerritoriesTests : GameRulesTests
        {
            [Fact]
            public void Can_not_attack()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingRegion, _currentPlayer, 2),
                    new Territory(_attackeeRegion, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingRegion, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }
    }
}