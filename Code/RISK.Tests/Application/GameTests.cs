using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Setup;
using RISK.Application.World;
using RISK.Tests.Application.Extensions;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application
{
    public abstract class GameTestsBase
    {
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly GameFactory _gameFactory;

        protected GameTestsBase()
        {
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();

            _gameFactory = new GameFactory(_cardFactory, _battle);
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _gameFactory.Create(gamePlaySetup);
        }

        public class InitializationTests : GameTestsBase
        {
            private readonly IGamePlaySetup _gameSetup;
            private readonly IPlayer _firstPlayer;
            private readonly ITerritory _territory;
            private readonly ITerritory _anotherTerritory;

            public InitializationTests()
            {
                _territory = Substitute.For<ITerritory>();
                _anotherTerritory = Substitute.For<ITerritory>();
                _firstPlayer = Substitute.For<IPlayer>();
                var anotherPlayer = Substitute.For<IPlayer>();

                _gameSetup = Make.GamePlaySetup
                    .WithTerritory(_territory)
                    .WithTerritory(_anotherTerritory)
                    .WithPlayer(_firstPlayer)
                    .WithPlayer(anotherPlayer)
                    .Build();
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
                var region = Substitute.For<IRegion>();
                var anotherRegion = Substitute.For<IRegion>();
                _territory.Region.Returns(region);
                _anotherTerritory.Region.Returns(anotherRegion);

                var sut = Create(_gameSetup);

                sut.GetTerritory(region).Should().Be(_territory);
                sut.GetTerritory(anotherRegion).Should().Be(_anotherTerritory);
            }

            [Fact]
            public void Can_not_attack()
            {
                var sut = Create(_gameSetup);

                sut.AssertCanNotAttack(_territory, _anotherTerritory);
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

                sut.AssertCanNotFortify(_territory, _anotherTerritory);
            }
        }

        public class CanAttackTests : GameTestsBase
        {
            private readonly IRegion _currentPlayerRegion = Substitute.For<IRegion>();
            private readonly IRegion _anotherPlayerRegion = Substitute.For<IRegion>();
            private readonly IPlayer _currentPlayer = Substitute.For<IPlayer>();
            private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
            private readonly ITerritory _currentPlayerTerritory = Substitute.For<ITerritory>();
            private readonly ITerritory _anotherPlayerTerritory = Substitute.For<ITerritory>();
            private readonly IGamePlaySetup _gamePlaySetup;

            public CanAttackTests()
            {
                _currentPlayerTerritory.Region.Returns(_currentPlayerRegion);
                _currentPlayerTerritory.Player.Returns(_currentPlayer);
                _anotherPlayerTerritory.Region.Returns(_anotherPlayerRegion);
                _anotherPlayerTerritory.Player.Returns(_anotherPlayer);

                _gamePlaySetup = Make.GamePlaySetup
                    .WithTerritory(_currentPlayerTerritory)
                    .WithTerritory(_anotherPlayerTerritory)
                    .WithPlayer(_currentPlayer)
                    .WithPlayer(_anotherPlayer)
                    .Build();
            }

            [Fact]
            public void Can_attack()
            {
                _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
                _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

                var sut = Create(_gamePlaySetup);

                sut.CanAttack(_currentPlayerTerritory, _anotherPlayerTerritory).Should().BeTrue();
            }

            [Fact]
            public void Can_not_attack_if_not_enough_attacking_armies()
            {
                _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
                _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(0);

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_currentPlayerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_not_attack_already_occupied_territory()
            {
                var occupiedTerritory = _anotherPlayerTerritory;
                var occupiedRegion = occupiedTerritory.Region;
                occupiedTerritory.Player.Returns(_currentPlayer);
                _currentPlayerRegion.HasBorder(occupiedRegion).Returns(true);
                _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_currentPlayerTerritory, occupiedTerritory);
            }

            [Fact]
            public void Can_not_attack_territory_without_having_border()
            {
                _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(false);
                _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_currentPlayerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                _anotherPlayerRegion.HasBorder(_currentPlayerRegion).Returns(true);
                _anotherPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_anotherPlayerTerritory, _currentPlayerTerritory);
            }
        }

        public class AttackTests : GameTestsBase
        {
            private readonly IRegion _currentPlayerRegion = Substitute.For<IRegion>();
            private readonly IRegion _anotherPlayerRegion = Substitute.For<IRegion>();
            private readonly IPlayer _currentPlayer = Substitute.For<IPlayer>();
            private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
            private readonly ITerritory _currentPlayerTerritory = Substitute.For<ITerritory>();
            private readonly ITerritory _anotherPlayerTerritory = Substitute.For<ITerritory>();
            private readonly IGamePlaySetup _gamePlaySetup;

            public AttackTests()
            {
                _currentPlayerTerritory.Region.Returns(_currentPlayerRegion);
                _currentPlayerTerritory.Player.Returns(_currentPlayer);
                _anotherPlayerTerritory.Region.Returns(_anotherPlayerRegion);
                _anotherPlayerTerritory.Player.Returns(_anotherPlayer);

                _gamePlaySetup = Make.GamePlaySetup
                    .WithTerritory(_currentPlayerTerritory)
                    .WithTerritory(_anotherPlayerTerritory)
                    .WithPlayer(_currentPlayer)
                    .WithPlayer(_anotherPlayer)
                    .Build();
            }

            [Fact]
            public void Attacks_but_territory_is_defended()
            {
                _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
                _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_currentPlayerTerritory, _anotherPlayerTerritory);

                _battle.Received().Attack(_currentPlayerTerritory, _anotherPlayerTerritory);
                sut.GetTerritory(_currentPlayerRegion).Player.Should().Be(_currentPlayer);
                sut.GetTerritory(_anotherPlayerRegion).Player.Should().Be(_anotherPlayer);
            }

            [Fact(Skip = "Not implemented")]
            public void Attacks_and_defeats_defender()
            {
                var sut = Create(_gamePlaySetup);
                sut.Attack(_currentPlayerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_move_armies_into_captured_territory()
            {
                var defenderIsDefeated = Substitute.For<IBattleResult>();
                defenderIsDefeated.IsDefenderDefeated().Returns(true);
                //GetAttackCandidatesReturns(_anotherPlayerTerritory);
                _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsDefeated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_currentPlayerTerritory, _anotherPlayerTerritory);

                sut.MustConfirmMoveOfArmiesIntoOccupiedTerritory().Should().BeTrue();
            }

            [Fact]
            public void Moves_armies_into_captured_territory()
            {
                var defenderIsEliminated = Substitute.For<IBattleResult>();
                defenderIsEliminated.IsDefenderDefeated().Returns(true);
                //GetAttackCandidatesReturns(_anotherPlayerTerritory);
                _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsEliminated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_currentPlayerTerritory, _anotherPlayerTerritory);
                sut.MoveArmiesIntoOccupiedTerritory(3);

                // Move to own test fixture
                // Test that canmove 
                // test move
                // (test to attack and standard move)
                // TBD: test that canmove prevents other actions
            }

            [Fact]
            public void Can_not_attack_before_move_into_captured_territory_has_been_confirmed()
            {
                var defenderIsEliminated = Substitute.For<IBattleResult>();
                defenderIsEliminated.IsDefenderDefeated().Returns(true);
                //GetAttackCandidatesReturns(_anotherPlayerTerritory);
                _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsEliminated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_currentPlayerTerritory, _anotherPlayerTerritory);

                sut.AssertCanNotAttack(_currentPlayerTerritory, _anotherPlayerTerritory);
            }
        }

        public class GameOverTests : GameTestsBase
        {
            [Fact]
            public void Is_game_over_when_all_territories_belongs_to_one_player()
            {
                var player = Substitute.For<IPlayer>();
                var gamePlaySetup = Make.GamePlaySetup
                    .WithTerritory(Make.Territory.Player(player).Build())
                    .WithTerritory(Make.Territory.Player(player).Build())
                    .Build();

                var sut = Create(gamePlaySetup);

                sut.IsGameOver().Should().BeTrue();
            }

            [Fact]
            public void Is_not_game_over_when_more_than_one_player_occupies_territories()
            {
                var gamePlaySetup = Make.GamePlaySetup
                    .WithTerritory(Make.Territory.Build())
                    .WithTerritory(Make.Territory.Build())
                    .Build();

                var sut = Create(gamePlaySetup);

                sut.IsGameOver().Should().BeFalse();
            }
        }

        public class TurnEndsTests : GameTestsBase
        {
            [Fact]
            public void End_turn_passes_turn_to_next_player()
            {
                var nextPlayer = Substitute.For<IPlayer>();
                var gameSetup = Make.GamePlaySetup
                    .WithPlayer(Substitute.For<IPlayer>())
                    .WithPlayer(nextPlayer)
                    .Build();

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
}