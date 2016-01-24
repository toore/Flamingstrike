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
        private readonly ITerritoryGeography _attackingTerritoryGeography;
        private readonly ITerritoryGeography _attackeeTerritoryGeography;
        private readonly GameRules _sut;

        protected GameRulesTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingTerritoryGeography = Substitute.For<ITerritoryGeography>();
            _attackeeTerritoryGeography = Substitute.For<ITerritoryGeography>();

            _sut = new GameRules();
        }

        public class GameRulesBorderingTerritoriesTests : GameRulesTests
        {
            private readonly ITerritoryGeography _secondAttackeeTerritoryGeography;

            public GameRulesBorderingTerritoriesTests()
            {
                _secondAttackeeTerritoryGeography = Substitute.For<ITerritoryGeography>();

                _attackingTerritoryGeography.HasBorder(_attackeeTerritoryGeography).Returns(true);
                _attackingTerritoryGeography.HasBorder(_secondAttackeeTerritoryGeography).Returns(true);
            }

            [Fact]
            public void Has_attackee_candidate()
            {
                var territories = new[]
                {
                    new Territory(_attackingTerritoryGeography, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryGeography, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryGeography, territories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryGeography);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryGeography, _currentPlayer, 1),
                    new Territory(_attackeeTerritoryGeography, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryGeography, gameboardTerritories);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryGeography, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryGeography, _otherPlayer, 1),
                    new Territory(_secondAttackeeTerritoryGeography, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryGeography, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryGeography, _secondAttackeeTerritoryGeography);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryGeography, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryGeography, _currentPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryGeography, gameboardTerritories);

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
                    new Territory(_attackingTerritoryGeography, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryGeography, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryGeography, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }
    }
}