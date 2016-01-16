using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;
using IPlayer = RISK.Application.IPlayer;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Application
{
    public abstract class GameRulesTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _otherPlayer;
        private readonly ITerritoryId _attackingTerritoryId;
        private readonly ITerritoryId _attackeeTerritoryId;
        private readonly GameRules _sut;

        protected GameRulesTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingTerritoryId = Substitute.For<ITerritoryId>();
            _attackeeTerritoryId = Substitute.For<ITerritoryId>();

            _sut = new GameRules();
        }

        public class GameRulesBorderingTerritoriesTests : GameRulesTests
        {
            private readonly ITerritoryId _secondAttackeeTerritoryId;

            public GameRulesBorderingTerritoriesTests()
            {
                _secondAttackeeTerritoryId = Substitute.For<ITerritoryId>();

                _attackingTerritoryId.HasBorderTo(_attackeeTerritoryId).Returns(true);
                _attackingTerritoryId.HasBorderTo(_secondAttackeeTerritoryId).Returns(true);
            }

            [Fact]
            public void Has_attackee_candidate()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryId);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayer, 1),
                    new Territory(_attackeeTerritoryId, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayer, 1),
                    new Territory(_secondAttackeeTerritoryId, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryId, _secondAttackeeTerritoryId);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryId, _currentPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

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
                    new Territory(_attackingTerritoryId, _currentPlayer, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }
    }
}