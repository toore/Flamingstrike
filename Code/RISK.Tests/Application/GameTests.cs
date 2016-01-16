﻿using System.Collections.Generic;
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
            private readonly IInGameplayPlayer _firstInGameplayPlayer;
            private readonly List<Territory> _territories;
            private readonly ITerritoryGeography _territoryGeography;
            private readonly ITerritoryGeography _anotherTerritoryGeography;

            public AfterInitializationTests()
            {
                _gameSetup = Make.GameSetup.Build();
                _firstInGameplayPlayer = Substitute.For<IInGameplayPlayer>();
                _playerFactory.Create(_gameSetup.Players).Returns(new List<IInGameplayPlayer>
                {
                    _firstInGameplayPlayer,
                    Substitute.For<IInGameplayPlayer>()
                });

                _territoryGeography = Substitute.For<ITerritoryGeography>();
                _anotherTerritoryGeography = Substitute.For<ITerritoryGeography>();
                _territories = new List<Territory>
                {
                    Make.Territory.TerritoryId(_territoryGeography).Build(),
                    Make.Territory.TerritoryId(_anotherTerritoryGeography).Build()
                };
                _territoryFactory.Create(_gameSetup.Territories).Returns(_territories);
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

                sut.Territories.Should().BeEquivalentTo(_territories);
            }

            [Fact]
            public void Can_not_attack()
            {
                var sut = Create(_gameSetup);

                sut.AssertCanNotAttack(_territoryGeography, _anotherTerritoryGeography);
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

                sut.AssertCanNotFortify(_territoryGeography, _anotherTerritoryGeography);
            }
        }

        public class AttackTests : GameTestsBase
        {
            private readonly ITerritoryGeography _playerTerritoryGeography = Substitute.For<ITerritoryGeography>();
            private readonly ITerritoryGeography _anotherPlayerTerritoryGeography = Substitute.For<ITerritoryGeography>();
            private readonly IPlayer _player = Substitute.For<IPlayer>();
            private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
            private readonly List<Territory> _territories;
            private readonly Territory _playerTerritory;
            private readonly Territory _anotherPlayerTerritory;

            public AttackTests()
            {
                var players = new List<IInGameplayPlayer>
                {
                    Make.Player.PlayerId(_player).Build(),
                    Make.Player.PlayerId(_anotherPlayer).Build()
                };
                _playerFactory.Create(null).ReturnsForAnyArgs(players);

                _playerTerritory = Make.Territory.TerritoryId(_playerTerritoryGeography).Player(_player).Build();
                _anotherPlayerTerritory = Make.Territory.TerritoryId(_anotherPlayerTerritoryGeography).Player(_anotherPlayer).Build();
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
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryGeography });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography).Should().BeTrue();
            }

            [Fact]
            public void Attacks()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryGeography });

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography);

                _battle.Received().Attack(_playerTerritory, _anotherPlayerTerritory);
            }

            [Fact]
            public void Can_not_attack()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new ITerritoryGeography[] { });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography);
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryGeography });

                var sut = Create(Make.GameSetup.Build());

                sut.AssertCanNotAttack(_anotherPlayerTerritoryGeography, _playerTerritoryGeography);
            }

            [Fact]
            public void Can_move_armies_into_captured_territory()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryGeography });
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(BattleResult.DefenderEliminated);

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography);

                sut.CanMoveArmiesIntoCapturedTerritory().Should().BeTrue();
            }

            [Fact]
            public void Can_not_attack_before_move_into_captured_territory_has_been_confirmed()
            {
                _gameRules.GetAttackeeCandidates(_playerTerritoryGeography, _territories)
                    .Returns(new[] { _anotherPlayerTerritoryGeography });
                _battle.Attack(_playerTerritory, _anotherPlayerTerritory)
                    .Returns(BattleResult.DefenderEliminated);

                var sut = Create(Make.GameSetup.Build());
                sut.Attack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography);

                sut.AssertCanNotAttack(_playerTerritoryGeography, _anotherPlayerTerritoryGeography);
            }
        }

        public class GameOverTests : GameTestsBase
        {
            [Fact]
            public void Is_game_over_when_all_territories_belongs_to_one_player()
            {
                var player = Substitute.For<IPlayer>();
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
                var gameSetup = Make.GameSetup.Build();
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