using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Setup;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Application
{
    public abstract class GameTestsBase
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly ITerritoryFactory _territoryFactory;
        private readonly GameFactory _gameFactory;
        private readonly IPlayerFactory _playerFactory;

        protected GameTestsBase()
        {
            _gameRules = Substitute.For<IGameRules>();
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();
            _territoryFactory = Substitute.For<ITerritoryFactory>();
            _playerFactory = Substitute.For<IPlayerFactory>();

            _gameFactory = new GameFactory(_gameRules, _cardFactory, _battle, _territoryFactory, _playerFactory);
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _gameFactory.Create(gamePlaySetup);
        }

        private void HasPlayers()
        {
            var players = new List<IPlayer>(new[] { Substitute.For<IPlayer>() });
            _playerFactory.Create(null).ReturnsForAnyArgs(players);
        }

        public class GameInitializationTests : GameTestsBase
        {
            [Fact]
            public void Initializes_with_first_player_taking_turn_first()
            {
                var gameSetup = Make.GameSetup.Build();
                var firstPlayer = Substitute.For<IPlayer>();
                _playerFactory.Create(gameSetup.Players).Returns(new List<IPlayer>
                {
                    firstPlayer,
                    Substitute.For<IPlayer>(),
                    Substitute.For<IPlayer>()
                });

                var sut = Create(gameSetup);

                sut.CurrentPlayer.Should().Be(firstPlayer);
            }

            [Fact]
            public void Initializes_territories()
            {
                var gameSetup = Make.GameSetup.Build();
                var expected = new List<Territory> { Make.Territory.Build() };
                _territoryFactory.Create(gameSetup.Territories).Returns(expected);
                HasPlayers();

                var sut = Create(gameSetup);

                sut.Territories.Should().BeEquivalentTo(expected);
            }
        }

        public class GameAttackTests : GameTestsBase
        {
            private readonly ITerritoryId _attackingTerritoryId = Substitute.For<ITerritoryId>();
            private readonly ITerritoryId _attackedTerritoryId = Substitute.For<ITerritoryId>();
            private readonly IPlayerId _playerId = Substitute.For<IPlayerId>();
            private readonly IPlayerId _anotherPlayerId = Substitute.For<IPlayerId>();

            public GameAttackTests()
            {
                var players = new List<IPlayer>
                {
                    Make.Player.PlayerId(_playerId).Build(),
                    Make.Player.PlayerId(_anotherPlayerId).Build()
                };
                _playerFactory.Create(null).ReturnsForAnyArgs(players);
            }

            [Fact]
            public void Can_attack()
            {
                var territories = HasTerritories(
                    Make.Territory.TerritoryId(_attackingTerritoryId).Player(_playerId).Build());
                _gameRules.GetAttackeeCandidates(_attackingTerritoryId, territories)
                    .Returns(new[] { _attackedTerritoryId });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(_attackingTerritoryId, _attackedTerritoryId).Should().BeTrue();
            }

            [Fact]
            public void Attack_moves_territories_into_()
            {
                throw new NotImplementedException();
            }

            [Fact]
            public void Can_not_attack()
            {
                var territories = HasTerritories(
                    Make.Territory.TerritoryId(_attackingTerritoryId).Player(_playerId).Build());
                _gameRules.GetAttackeeCandidates(_attackingTerritoryId, territories)
                    .Returns(new ITerritoryId[] { });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_attackingTerritoryId, _attackedTerritoryId);
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                var territories = HasTerritories(
                    Make.Territory.TerritoryId(_attackingTerritoryId).Player(_anotherPlayerId).Build());
                _gameRules.GetAttackeeCandidates(_attackingTerritoryId, territories)
                    .Returns(new[] { _attackedTerritoryId });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_attackingTerritoryId, _attackedTerritoryId);
            }

            private List<Territory> HasTerritories(params Territory[] territories)
            {
                var result = territories.ToList();
                _territoryFactory.Create(null).ReturnsForAnyArgs(result);

                return result;
            }
        }

        public class GameGameOverTests : GameTestsBase
        {
            [Fact]
            public void Is_game_over_when_all_territories_belongs_to_one_player()
            {
                var player = Substitute.For<IPlayerId>();
                _territoryFactory.Create(null).ReturnsForAnyArgs(new List<Territory>
                {
                    Make.Territory.Player(player).Build(),
                    Make.Territory.Player(player).Build()
                });
                HasPlayers();

                var sut = Create(Make.GameSetup.Build());

                sut.IsGameOver().Should().BeTrue();
            }

            [Fact]
            public void Is_not_game_over_when_more_than_one_player_occupies_territories()
            {
                _territoryFactory.Create(null).ReturnsForAnyArgs(new List<Territory>
                {
                    Make.Territory.Build(),
                    Make.Territory.Build()
                });
                HasPlayers();

                var sut = Create(Make.GameSetup.Build());

                sut.IsGameOver().Should().BeFalse();
            }
        }

        public class GameTurnEndsTests : GameTestsBase
        {
            [Fact]
            public void End_turn_passes_turn_to_next_player()
            {
                var nextPlayer = Substitute.For<IPlayer>();
                var gameSetup = Make.GameSetup.Build();
                _playerFactory.Create(gameSetup.Players).Returns(new List<IPlayer>
                {
                    Substitute.For<IPlayer>(),
                    nextPlayer
                });

                var sut = Create(gameSetup);
                sut.EndTurn();

                sut.CurrentPlayer.Should().Be(nextPlayer);
            }

            //[Fact]
            //public void Player_should_receive_card_when_turn_ends()
            //{
            //    //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            //    var card = Make.Card.Build();
            //    _cardFactory.Create().Returns(card);

            //    _sut.EndTurn();

            //    //_currentPlayerId.Received().AddCard(card);
            //    throw new NotImplementedException();
            //}

            //[Fact]
            //public void Player_should_not_receive_card_when_turn_ends()
            //{
            //    //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            //    _sut.EndTurn();

            //    //_currentPlayerId.DidNotReceiveWithAnyArgs().AddCard(null);
            //    throw new NotImplementedException();
            //}
        }
    }

    public static class GameTestsExtensions
    {
        public static void AssertCanNotAttack(this IGame sut, ITerritoryId attackingTerritoryId, ITerritoryId attackedTerritoryId)
        {
            Action act = () => sut.Attack(attackingTerritoryId, attackedTerritoryId);

            sut.CanAttack(attackingTerritoryId, attackedTerritoryId).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}