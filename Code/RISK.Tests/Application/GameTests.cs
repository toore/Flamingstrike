using System;
using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
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

        public class AfterInitializationTests : GameTestsBase
        {
            private readonly IGamePlaySetup _gameSetup;
            private readonly IPlayer _firstPlayer;
            private readonly List<Territory> _territories;
            private readonly ITerritoryId _territoryId;
            private readonly ITerritoryId _anotherTerritoryId;

            public AfterInitializationTests()
            {
                _gameSetup = Make.GameSetup.Build();
                _firstPlayer = Substitute.For<IPlayer>();
                _playerFactory.Create(_gameSetup.Players).Returns(new List<IPlayer>
                {
                    _firstPlayer,
                    Substitute.For<IPlayer>()
                });

                _territoryId = Substitute.For<ITerritoryId>();
                _anotherTerritoryId = Substitute.For<ITerritoryId>();
                _territories = new List<Territory>
                {
                    Make.Territory.TerritoryId(_territoryId).Build(),
                    Make.Territory.TerritoryId(_anotherTerritoryId).Build()
                };
                _territoryFactory.Create(_gameSetup.Territories).Returns(_territories);
            }

            [Fact]
            public void Initializes_with_first_player_taking_turn_first()
            {
                var sut = Create(_gameSetup);

                sut.CurrentPlayer.Should().Be(_firstPlayer);
            }

            [Fact]
            public void Initializes_territories()
            {
                var sut = Create(_gameSetup);

                sut.Territories.Should().BeEquivalentTo(_territories);
            }

            [Fact]
            public void Can_not_attack()
            {
                var sut = Create(_gameSetup);

                sut.AssertCanNotAttack(_territoryId, _anotherTerritoryId);
            }

            [Fact]
            public void Can_not_move_armies_into_captured_territory()
            {
                var sut = Create(_gameSetup);

                sut.AssertCanNotMoveArmiesIntoCapturedTerritory(1);
            }

            [Fact]
            public void Can_not_fortify()
            {
                var sut = Create(_gameSetup);

                sut.AssertCanNotFortify(_territoryId, _anotherTerritoryId);
            }
        }

        public class AttackTests : GameTestsBase
        {
            private readonly ITerritoryId _playerTerritoryId = Substitute.For<ITerritoryId>();
            private readonly ITerritoryId _anotherPlayerTerritoryId = Substitute.For<ITerritoryId>();
            private readonly IPlayerId _playerId = Substitute.For<IPlayerId>();
            private readonly IPlayerId _anotherPlayerId = Substitute.For<IPlayerId>();
            private readonly List<Territory> _territories;
            private readonly Territory _playerTerritory;
            private readonly Territory _anotherPlayerTerritory;

            public AttackTests()
            {
                var players = new List<IPlayer>
                {
                    Make.Player.PlayerId(_playerId).Build(),
                    Make.Player.PlayerId(_anotherPlayerId).Build()
                };
                _playerFactory.Create(null).ReturnsForAnyArgs(players);

                _playerTerritory = Make.Territory.TerritoryId(_playerTerritoryId).Player(_playerId).Build();
                _anotherPlayerTerritory = Make.Territory.TerritoryId(_anotherPlayerTerritoryId).Player(_anotherPlayerId).Build();
                _territories = new List<Territory>
                {
                    _playerTerritory,
                    _anotherPlayerTerritory
                };
                _territoryFactory.Create(null).ReturnsForAnyArgs(_territories);
            }

            [Fact]
            public void Can_attack()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryId });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(_playerTerritoryId, _anotherPlayerTerritoryId).Should().BeTrue();
            }

            [Fact]
            public void Attacks()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryId });

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryId, _anotherPlayerTerritoryId);

                _battle.Received().Attack(_playerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_not_attack()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new ITerritoryId[] { });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_playerTerritoryId, _anotherPlayerTerritoryId);
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryId });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_anotherPlayerTerritoryId, _playerTerritoryId);
            }

            [Fact]
            public void Can_move_armies_into_captured_territory()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryId });
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(BattleResult.DefenderEliminated);

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryId, _anotherPlayerTerritoryId);

                sut.CanMoveArmiesIntoCapturedTerritory().Should().BeTrue();
            }

            [Fact]
            public void Can_not_attack_before_move_into_captured_territory_has_been_confirmed()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryId, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryId });
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(BattleResult.DefenderEliminated);

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryId, _anotherPlayerTerritoryId);

                sut.AssertCanNotAttack(_playerTerritoryId, _anotherPlayerTerritoryId);
            }
        }

        public class GameOverTests : GameTestsBase
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

            private void HasPlayers()
            {
                var players = new List<IPlayer>(new[] { Substitute.For<IPlayer>() });
                _playerFactory.Create(null).ReturnsForAnyArgs(players);
            }
        }

        public class TurnEndsTests : GameTestsBase
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

        public static void AssertCanNotMoveArmiesIntoCapturedTerritory(this IGame sut, int numberOfArmies)
        {
            Action act = () => sut.MoveArmiesIntoCapturedTerritory(numberOfArmies);

            sut.CanMoveArmiesIntoCapturedTerritory().Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotFortify(this IGame sut, ITerritoryId attackingTerritoryId, ITerritoryId attackedTerritoryId)
        {
            Action act = () => sut.Fortify(attackingTerritoryId, attackedTerritoryId);

            sut.CanFortify(attackingTerritoryId, attackedTerritoryId).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }

    public class GameTestsExtensionsTests
    {
        private readonly IGame _sut;
        private readonly ITerritoryId _territoryId;
        private readonly ITerritoryId _anotherTerritoryId;
        private const int _numberOfArmies = 1;

        public GameTestsExtensionsTests()
        {
            _territoryId = Substitute.For<ITerritoryId>();
            _anotherTerritoryId = Substitute.For<ITerritoryId>();

            _sut = Substitute.For<IGame>();
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_CanAttack_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotAttack(_territoryId, _anotherTerritoryId),
                x => x.CanAttack(_territoryId, _anotherTerritoryId),
                x => x.Attack(_territoryId, _anotherTerritoryId));
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_Attack_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotAttack(_territoryId, _anotherTerritoryId));
        }

        [Fact]
        public void AssertCanNotMoveArmiesIntoCapturedTerritory_asserts_that_CanMoveArmiesIntoCapturedTerritory_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies),
                x => x.CanMoveArmiesIntoCapturedTerritory(),
                x => x.MoveArmiesIntoCapturedTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanMoveArmiesIntoCapturedTerritory_asserts_that_MoveArmiesIntoCapturedTerritory_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_CanFortify_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotFortify(_territoryId, _anotherTerritoryId),
                x => x.CanFortify(_territoryId, _anotherTerritoryId),
                x => x.Fortify(_territoryId, _anotherTerritoryId));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_Fortify_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotFortify(_territoryId, _anotherTerritoryId));
        }

        private void AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
            Action<IGame> assertAction,
            Func<IGame, bool> isEnabled,
            Action<IGame> action)
        {
            isEnabled.Invoke(_sut).Returns(true);
            _sut.When(action).Throw<InvalidOperationException>();

            AssertMethodThrowsAssertionFailedException(assertAction);
        }

        private void AssertMethodThrowsAssertionFailedException(Action<IGame> assertAction)
        {
            Action act = () => assertAction.Invoke(_sut);
            act.ShouldThrow<AssertionFailedException>();
        }
    }
}