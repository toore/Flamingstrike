using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application
{
    public abstract class GameStateTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _otherPlayer;
        private readonly ITerritory _attackingTerritory;
        private readonly ITerritory _attackeeTerritory;

        protected GameStateTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _otherPlayer = Substitute.For<IPlayer>();
            _attackingTerritory = Substitute.For<ITerritory>();
            _attackeeTerritory = Substitute.For<ITerritory>();
        }

        public class Bordering_territories : GameStateTests
        {
            public Bordering_territories()
            {
                _attackingTerritory.HasBorderTo(_attackeeTerritory).Returns(true);
            }

            [Fact]
            public void Has_attack_candidate()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1)
                };
                var sut = new GameState
                {
                    CurrentPlayer = _currentPlayer,
                    Territories = gameboardTerritories
                };

                sut.GetAttackCandidates(_attackingTerritory).Should().BeEquivalentTo(_attackeeTerritory);
            }

            [Fact]
            public void Has_attack_candidates()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _otherPlayer, 1)
                };
                var sut = new GameState
                {
                    CurrentPlayer = _currentPlayer,
                    Territories = gameboardTerritories
                };

                sut.GetAttackCandidates(_attackingTerritory).Should().BeEquivalentTo(_attackeeTerritory);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _currentPlayer, 1)
                };
                var sut = new GameState
                {
                    CurrentPlayer = _currentPlayer,
                    Territories = gameboardTerritories
                };

                sut.GetAttackCandidates(_attackingTerritory).Should().BeEmpty();
            }
        }

        public class Orphaned_territories : GameStateTests
        {
            [Fact]
            public void Can_not_attack()
            {
                var gameboardTerritories = new[]
                {
                    new GameboardTerritory(_attackingTerritory, _currentPlayer, 2),
                    new GameboardTerritory(_attackeeTerritory, _currentPlayer, 1)
                };
                var sut = new GameState
                {
                    CurrentPlayer = _currentPlayer,
                    Territories = gameboardTerritories
                };

                sut.GetAttackCandidates(_attackingTerritory).Should().BeEmpty();
            }
        }
    }
}