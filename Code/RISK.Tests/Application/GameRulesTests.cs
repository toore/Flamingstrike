using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Application
{
    public class GameRulesTests
    {
        private readonly IPlayerId _currentPlayerId;
        private readonly IPlayerId _otherPlayerId;
        private readonly ITerritoryId _attackingTerritoryId;
        private readonly ITerritoryId _attackeeTerritoryId;
        private readonly GameRules _sut;

        protected GameRulesTests()
        {
            _currentPlayerId = Substitute.For<IPlayerId>();
            _otherPlayerId = Substitute.For<IPlayerId>();
            _attackingTerritoryId = Substitute.For<ITerritoryId>();
            _attackeeTerritoryId = Substitute.For<ITerritoryId>();

            _sut = new GameRules();
        }

        public class Bordering_territories : GameRulesTests
        {
            private readonly ITerritoryId _secondAttackeeTerritoryId;

            public Bordering_territories()
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
                    new Territory(_attackingTerritoryId, _currentPlayerId, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayerId, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryId);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayerId, 1),
                    new Territory(_attackeeTerritoryId, _otherPlayerId, 1),
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayerId, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayerId, 1),
                    new Territory(_secondAttackeeTerritoryId, _otherPlayerId, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEquivalentTo(_attackeeTerritoryId, _secondAttackeeTerritoryId);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayerId, 2),
                    new Territory(_attackeeTerritoryId, _currentPlayerId, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }

        public class Orphaned_territories : GameRulesTests
        {
            [Fact]
            public void Can_not_attack()
            {
                var gameboardTerritories = new[]
                {
                    new Territory(_attackingTerritoryId, _currentPlayerId, 2),
                    new Territory(_attackeeTerritoryId, _otherPlayerId, 1)
                };

                var actual = _sut.GetAttackeeCandidates(_attackingTerritoryId, gameboardTerritories);

                actual.Should().BeEmpty();
            }
        }
    }
}