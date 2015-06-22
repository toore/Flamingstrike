using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameboardRulesTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _otherPlayer;
        private readonly ITerritory _attackingTerritory;
        private readonly ITerritory _attackeeTerritory;
        private readonly GameboardRules _sut;

        protected GameboardRulesTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingTerritory = Substitute.For<ITerritory>();
            _attackeeTerritory = Substitute.For<ITerritory>();

            _sut = new GameboardRules();
        }

        public class Bordering_territories : GameboardRulesTests
        {
            private readonly ITerritory _secondAttackeeTerritory;

            public Bordering_territories()
            {
                _secondAttackeeTerritory = Substitute.For<ITerritory>();

                _attackingTerritory.HasBorderTo(_attackeeTerritory).Returns(true);
                _attackingTerritory.HasBorderTo(_secondAttackeeTerritory).Returns(true);
            }

            [Fact]
            public void Has_attackee_candidate()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(gameboardTerritories, _attackingTerritory);

                actual.Should().BeEquivalentTo(_attackeeTerritory);
            }

            [Fact]
            public void Can_not_attack_if_not_enough_armies()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 1),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1),
                };

                var actual = _sut.GetAttackeeCandidates(gameboardTerritories, _attackingTerritory);

                actual.Should().BeEmpty();
            }

            [Fact]
            public void Has_attackee_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1),
                    new GameboardTerritory(_secondAttackeeTerritory, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(gameboardTerritories, _attackingTerritory);

                actual.Should().BeEquivalentTo(_attackeeTerritory, _secondAttackeeTerritory);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _currentPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(gameboardTerritories, _attackingTerritory);

                actual.Should().BeEmpty();
            }
        }

        public class Orphaned_territories : GameboardRulesTests
        {
            [Fact]
            public void Can_not_attack()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1)
                };

                var actual = _sut.GetAttackeeCandidates(gameboardTerritories, _attackingTerritory);

                actual.Should().BeEmpty();
            }
        }
    }
}