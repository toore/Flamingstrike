using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GuiWpf.Extensions;
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
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly GameFactory _gameFactory;
        private readonly IPlayerFactory _playerFactory;

        protected GameTestsBase()
        {
            _gameRules = Substitute.For<IGameRules>();
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();
            _playerFactory = Substitute.For<IPlayerFactory>();

            _gameFactory = new GameFactory(_gameRules, _cardFactory, _battle, _playerFactory);
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _gameFactory.Create(gamePlaySetup);
        }

        public class AfterInitializationTests : GameTestsBase
        {
            private readonly IGamePlaySetup _gameSetup;
            private readonly IInGameplayPlayer _firstInGameplayPlayer;
            private readonly ITerritory _territory;
            private readonly ITerritory _anotherTerritory;

            public AfterInitializationTests()
            {
                _territory = Substitute.For<ITerritory>();
                _anotherTerritory = Substitute.For<ITerritory>();

                _gameSetup = Make.GamePlaySetup
                    .WithTerritory(_territory)
                    .WithTerritory(_anotherTerritory)
                    .Build();
                _firstInGameplayPlayer = Substitute.For<IInGameplayPlayer>();
                _playerFactory.Create(_gameSetup.Players).Returns(new List<IInGameplayPlayer>
                {
                    _firstInGameplayPlayer,
                    Substitute.For<IInGameplayPlayer>()
                });
            }

            [Fact]
            public void Initializes_with_first_player_taking_turn_first()
            {
                var sut = Create(_gameSetup);

                sut.CurrentPlayer.Should().Be(_firstInGameplayPlayer);
            }

            [Fact]
            public void Initializes_territories()
            {
                var sut = Create(_gameSetup);

                sut.GetTerritories().Should().BeEquivalentTo(_territory, _anotherTerritory);
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

        public class AttackTests : GameTestsBase
        {
            private readonly IRegion _playerRegion = Substitute.For<IRegion>();
            private readonly IRegion _anotherPlayerRegion = Substitute.For<IRegion>();
            private readonly IPlayer _player = Substitute.For<IPlayer>();
            private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
            private readonly ITerritory _playerTerritory;
            private readonly ITerritory _anotherPlayerTerritory;
            private readonly IGamePlaySetup _gamePlaySetup;

            public AttackTests()
            {
                var players = new List<IInGameplayPlayer>
                {
                    Make.Player.PlayerId(_player).Build(),
                    Make.Player.PlayerId(_anotherPlayer).Build()
                };
                _playerFactory.Create(null).ReturnsForAnyArgs(players);

                _playerTerritory = Make.Territory.TerritoryGeography(_playerRegion).Player(_player).Build();
                _anotherPlayerTerritory = Make.Territory.TerritoryGeography(_anotherPlayerRegion).Player(_anotherPlayer).Build();
                _gamePlaySetup = Make.GamePlaySetup
                    .WithTerritory(_playerTerritory)
                    .WithTerritory(_anotherPlayerTerritory)
                    .Build();
            }

            [Fact]
            public void Can_attack()
            {
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);

                var sut = Create(_gamePlaySetup);

                sut.CanAttack(_playerTerritory, _anotherPlayerTerritory).Should().BeTrue();
            }

            [Fact]
            public void Attacks()
            {
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_playerTerritory, _anotherPlayerTerritory);

                _battle.Received().Attack(_playerTerritory, _anotherPlayerTerritory);
                sut.GetTerritory(_playerTerritory.Region).Player.Should().Be(_player);
                sut.GetTerritory(_anotherPlayerTerritory.Region).Player.Should().Be(_anotherPlayer);
            }

            [Fact(Skip = "Not implemented")]
            public void Attacks_and_updates_armies()
            {
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_playerTerritory, _anotherPlayerTerritory);

                
            }

            [Fact]
            public void Can_not_attack()
            {
                GetAttackeeCandidatesReturns();

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_playerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);

                var sut = Create(_gamePlaySetup);

                sut.AssertCanNotAttack(_anotherPlayerTerritory, _playerTerritory);
            }

            [Fact]
            public void Can_move_armies_into_captured_territory()
            {
                var defenderIsEliminated = Substitute.For<IBattleResult>();
                defenderIsEliminated.IsDefenderEliminated().Returns(true);
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsEliminated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_playerTerritory, _anotherPlayerTerritory);

                sut.CanMoveArmiesIntoCapturedTerritory().Should().BeTrue();
            }

            [Fact]
            public void Moves_armies_into_captured_territory()
            {
                var defenderIsEliminated = Substitute.For<IBattleResult>();
                defenderIsEliminated.IsDefenderEliminated().Returns(true);
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsEliminated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_playerTerritory, _anotherPlayerTerritory);
                sut.MoveArmiesIntoCapturedTerritory(3);

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
                defenderIsEliminated.IsDefenderEliminated().Returns(true);
                GetAttackeeCandidatesReturns(_anotherPlayerRegion);
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(defenderIsEliminated);

                var sut = Create(_gamePlaySetup);
                sut.Attack(_playerTerritory, _anotherPlayerTerritory);

                sut.AssertCanNotAttack(_playerTerritory, _anotherPlayerTerritory);
            }

            private void GetAttackeeCandidatesReturns(params IRegion[] regions)
            {
                _gameRules.GetAttackeeCandidates(
                    _playerRegion, ArgListIsEquivalentTo(_playerTerritory, _anotherPlayerTerritory))
                    .Returns(regions);
            }

            private static IReadOnlyList<T> ArgListIsEquivalentTo<T>(params T[] equivalency)
            {
                return Arg.Is<IReadOnlyList<T>>(arg => IsEquivalentTo(arg, equivalency));
            }

            private static bool IsEquivalentTo<T>(IEnumerable<T> arg, params T[] equivalency)
            {
                return arg.Except(equivalency).IsEmpty();
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
                HasPlayers();

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
                HasPlayers();

                var sut = Create(gamePlaySetup);

                sut.IsGameOver().Should().BeFalse();
            }

            private void HasPlayers()
            {
                var players = new List<IInGameplayPlayer>(new[] { Substitute.For<IInGameplayPlayer>() });
                _playerFactory.Create(null).ReturnsForAnyArgs(players);
            }
        }

        public class TurnEndsTests : GameTestsBase
        {
            [Fact]
            public void End_turn_passes_turn_to_next_player()
            {
                var nextPlayer = Substitute.For<IInGameplayPlayer>();
                var gameSetup = Make.GamePlaySetup.Build();
                _playerFactory.Create(gameSetup.Players).Returns(new List<IInGameplayPlayer>
                {
                    Substitute.For<IInGameplayPlayer>(),
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
}