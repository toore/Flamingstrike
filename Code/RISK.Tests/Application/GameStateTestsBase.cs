using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.GamePhases;
using RISK.Application.Setup;
using RISK.Application.World;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
using Xunit;

namespace RISK.Tests.Application
{
    public abstract class GameStateTestsBase
    {
        [Fact]
        public void Gets_current_player()
        {
            var currentPlayer = Substitute.For<IPlayer>();
            var gameData = Make.GameData
                .CurrentPlayer(currentPlayer)
                .Build();

            var sut = Create(gameData);

            sut.CurrentPlayer.Should().Be(currentPlayer);
        }

        protected abstract IGameState Create(GameData gameData);

        [Fact]
        public void Gets_territory()
        {
            var territory = Substitute.For<ITerritory>();
            var region = Substitute.For<IRegion>();
            territory.Region.Returns(region);
            var gameData = Make.GameData
                .WithTerritory(territory)
                .WithTerritory(Substitute.For<ITerritory>())
                .WithTerritory(Substitute.For<ITerritory>())
                .Build();

            var sut = Create(gameData);

            sut.GetTerritory(region).Should().Be(territory);
        }

        public class DraftArmiesGameStateTests : GameStateTestsBase
        {
            private readonly IGameStateFactory _gameStateFactory;
            private readonly ITerritoryUpdater _territoryUpdater;

            private readonly ITerritory _territory;
            private readonly ITerritory _anotherTerritory;
            private readonly IRegion _region;
            private readonly IRegion _anotherRegion;
            private readonly IPlayer _currentPlayer;
            private readonly IPlayer _anotherPlayer;
            private readonly GameData _gameData;

            public DraftArmiesGameStateTests()
            {
                _gameStateFactory = Substitute.For<IGameStateFactory>();
                _territoryUpdater = Substitute.For<ITerritoryUpdater>();

                _territory = Substitute.For<ITerritory>();
                _anotherTerritory = Substitute.For<ITerritory>();
                _currentPlayer = Substitute.For<IPlayer>();
                _anotherPlayer = Substitute.For<IPlayer>();

                _region = Substitute.For<IRegion>();
                _anotherRegion = Substitute.For<IRegion>();
                _territory.Region.Returns(_region);
                _territory.Player.Returns(_currentPlayer);
                _anotherTerritory.Region.Returns(_anotherRegion);

                _gameData = Make.GameData
                    .CurrentPlayer(_currentPlayer)
                    .WithPlayer(_currentPlayer)
                    .WithPlayer(_anotherPlayer)
                    .WithTerritory(_territory)
                    .WithTerritory(_anotherTerritory)
                    .Build();
            }

            [Theory, AutoData]
            public void Gets_number_of_armies_to_draft(int numberOfArmiesToDraft)
            {
                var sut = Create(_gameData, numberOfArmiesToDraft);

                sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmiesToDraft);
            }

            [Fact]
            public void Can_place_draft_armies_for_occupied_territory()
            {
                var sut = Create(_gameData);

                sut.CanPlaceDraftArmies(_region).Should().BeTrue();
            }

            [Fact]
            public void Can_not_place_draft_armies_for_another_territory()
            {
                var sut = Create(_gameData);

                sut.CanPlaceDraftArmies(_anotherRegion).Should().BeFalse();
            }

            [Fact]
            public void Placing_draft_armies_returns_draft_armies_game_state()
            {
                var updatedTerritories = new List<ITerritory> { Make.Territory.Build() };
                var draftArmiesGameState = Substitute.For<IGameState>();
                _territoryUpdater
                    .PlaceArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                    .Returns(updatedTerritories);
                _gameStateFactory.CreateDraftArmiesGameState(Arg.Is<GameData>(x =>
                    x.CurrentPlayer == _currentPlayer
                    &&
                    x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                    &&
                    x.Territories.IsEquivalent(updatedTerritories)
                    ),
                    1)
                    .Returns(draftArmiesGameState);

                var sut = Create(_gameData, 3);
                var result = sut.PlaceDraftArmies(_region, 2);

                result.Should().Be(draftArmiesGameState);
            }

            [Fact]
            public void Placing_all_draft_armies_returns_attack_game_state()
            {
                var updatedTerritories = new List<ITerritory> { Make.Territory.Build() };
                var attackGameState = Substitute.For<IGameState>();
                _territoryUpdater
                    .PlaceArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                    .Returns(updatedTerritories);
                _gameStateFactory.CreateAttackGameState(Arg.Is<GameData>(x =>
                    x.CurrentPlayer == _currentPlayer
                    &&
                    x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                    &&
                    x.Territories.IsEquivalent(updatedTerritories)))
                    .Returns(attackGameState);

                var sut = Create(_gameData, 2);
                var result = sut.PlaceDraftArmies(_region, 2);

                result.Should().Be(attackGameState);
            }

            [Fact]
            public void Placing_more_draft_armies_than_left_throws()
            {
                var sut = Create(_gameData);

                Action act = () => sut.PlaceDraftArmies(_region, 2);

                act.ShouldThrow<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Can_not_attack()
            {
                var sut = Create(_gameData);

                sut.CanAttack(_region, _anotherRegion).Should().BeFalse();
            }

            [Fact]
            public void Attack_throws()
            {
                var sut = Create(_gameData);

                Action act = () => sut.Attack(_region, _anotherRegion);

                act.ShouldThrow<InvalidOperationException>();
            }

            [Fact]
            public void Can_not_send_armies_to_occupy()
            {
                var sut = Create(_gameData);

                sut.CanSendArmiesToOccupy().Should().BeFalse();
            }

            [Fact]
            public void Sending_armies_to_occupy_throws()
            {
                var sut = Create(_gameData);

                Action act = () => sut.SendArmiesToOccupy(1);

                act.ShouldThrow<InvalidOperationException>();
            }

            [Fact]
            public void Can_not_fortify()
            {
                var sut = Create(_gameData);

                sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
            }

            [Fact]
            public void Fortifying_throws()
            {
                var sut = Create(_gameData);

                Action act = () => sut.Fortify(_region, _anotherRegion, 1);

                act.ShouldThrow<InvalidOperationException>();
            }

            [Fact]
            public void Can_not_end_turn()
            {
                var sut = Create(_gameData);

                sut.CanEndTurn().Should().BeFalse();
            }

            [Fact]
            public void Ending_turn_throws()
            {
                var sut = Create(_gameData);

                Action act = () => sut.EndTurn();

                act.ShouldThrow<InvalidOperationException>();
            }

            protected override IGameState Create(GameData gameData)
            {
                return Create(gameData, 1);
            }

            private IGameState Create(GameData gameData, int numberOfArmiesToDraft)
            {
                return new DraftArmiesGameState(_gameStateFactory, _territoryUpdater, gameData, numberOfArmiesToDraft);
            }
        }

        //public class CanAttackTests : GameStateTestsBase
        //{
        //    private readonly IRegion _currentPlayerRegion = Substitute.For<IRegion>();
        //    private readonly IRegion _anotherPlayerRegion = Substitute.For<IRegion>();
        //    private readonly IPlayer _currentPlayer = Substitute.For<IPlayer>();
        //    private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
        //    private readonly ITerritory _currentPlayerTerritory = Substitute.For<ITerritory>();
        //    private readonly ITerritory _anotherPlayerTerritory = Substitute.For<ITerritory>();
        //    private readonly IGamePlaySetup _gamePlaySetup;

        //    public CanAttackTests()
        //    {
        //        _currentPlayerTerritory.Region.Returns(_currentPlayerRegion);
        //        _currentPlayerTerritory.Player.Returns(_currentPlayer);
        //        _anotherPlayerTerritory.Region.Returns(_anotherPlayerRegion);
        //        _anotherPlayerTerritory.Player.Returns(_anotherPlayer);

        //        _gamePlaySetup = Make.GamePlaySetup
        //            .WithTerritory(_currentPlayerTerritory)
        //            .WithTerritory(_anotherPlayerTerritory)
        //            .WithPlayer(_currentPlayer)
        //            .WithPlayer(_anotherPlayer)
        //            .Build();
        //    }

        //[Fact]
        //public void Can_attack()
        //{
        //    _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
        //    _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

        //    var sut = Create(_gamePlaySetup);

        //    sut.CanAttack(_currentPlayerRegion, _anotherPlayerRegion).Should().BeTrue();
        //}

        //[Fact]
        //public void Can_not_attack_if_not_enough_attacking_armies()
        //{
        //    _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
        //    _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(0);

        //    var sut = Create(_gamePlaySetup);

        //    sut.AssertCanNotAttack(_currentPlayerRegion, _anotherPlayerRegion);
        //}

        //[Fact]
        //public void Can_not_attack_already_occupied_territory()
        //{
        //    var occupiedTerritory = _anotherPlayerTerritory;
        //    var occupiedRegion = occupiedTerritory.Region;
        //    occupiedTerritory.Player.Returns(_currentPlayer);
        //    _currentPlayerRegion.HasBorder(occupiedRegion).Returns(true);
        //    _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

        //    var sut = Create(_gamePlaySetup);

        //    sut.AssertCanNotAttack(_currentPlayerRegion, occupiedRegion);
        //}

        //[Fact]
        //public void Can_not_attack_territory_without_having_border()
        //{
        //    _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(false);
        //    _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

        //    var sut = Create(_gamePlaySetup);

        //    sut.AssertCanNotAttack(_currentPlayerRegion, _anotherPlayerRegion);
        //}

        //[Fact]
        //public void Can_not_attack_with_another_players_territory()
        //{
        //    _anotherPlayerRegion.HasBorder(_currentPlayerRegion).Returns(true);
        //    _anotherPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

        //    var sut = Create(_gamePlaySetup);

        //    sut.AssertCanNotAttack(_anotherPlayerRegion, _currentPlayerRegion);
        //}
        //}

        public class AttackTests : GameStateTestsBase
        {
            private readonly IGameStateFactory _gameStateFactory;
            private readonly IBattle _battle;
            private readonly IArmyDraftCalculator _armyDraftCalculator;

            private readonly IRegion _currentPlayerRegion = Substitute.For<IRegion>();
            private readonly IRegion _anotherPlayerRegion = Substitute.For<IRegion>();
            private readonly IPlayer _currentPlayer = Substitute.For<IPlayer>();
            private readonly IPlayer _anotherPlayer = Substitute.For<IPlayer>();
            private readonly ITerritory _currentPlayerTerritory = Substitute.For<ITerritory>();
            private readonly ITerritory _anotherPlayerTerritory = Substitute.For<ITerritory>();
            private readonly IGamePlaySetup _gamePlaySetup;

            public AttackTests()
            {
                _gameStateFactory = Substitute.For<IGameStateFactory>();
                _battle = Substitute.For<IBattle>();
                _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();

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

            //[Fact]
            //public void Attacks_but_territory_is_defended()
            //{
            //    _currentPlayerRegion.HasBorder(_anotherPlayerRegion).Returns(true);
            //    _currentPlayerTerritory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            //    var sut = Create(_gamePlaySetup);
            //    sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);

            //    _battle.Received().Attack(_currentPlayerTerritory, _anotherPlayerTerritory);
            //    sut.GetTerritory(_currentPlayerRegion).Player.Should().Be(_currentPlayer);
            //    sut.GetTerritory(_anotherPlayerRegion).Player.Should().Be(_anotherPlayer);
            //}

            //[Fact(Skip = "Not implemented")]
            //public void Attacks_and_defeats_defender()
            //{
            //    var sut = Create(_gamePlaySetup);
            //    sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);
            //}

            //[Fact(Skip = "Not implemented")]
            //public void Can_move_armies_into_captured_territory()
            //{
            //    var defenderIsDefeated = Substitute.For<IBattleResult>();
            //    defenderIsDefeated.IsDefenderDefeated().Returns(true);
            //    //GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //    _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //        .Returns(defenderIsDefeated);

            //    var sut = Create(_gamePlaySetup);
            //    sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);

            //    sut.CanSendArmiesToOccupy().Should().BeTrue();
            //}

            //[Fact(Skip = "Not implemented")]
            //public void Moves_armies_into_captured_territory()
            //{
            //    var defenderIsEliminated = Substitute.For<IBattleResult>();
            //    defenderIsEliminated.IsDefenderDefeated().Returns(true);
            //    //GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //    _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //        .Returns(defenderIsEliminated);

            //    var sut = Create(_gamePlaySetup);
            //    sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);
            //    sut.SendArmiesToOccupy(3);

            //    // Move to own test fixture
            //    // Test that canmove 
            //    // test move
            //    // (test to attack and standard move)
            //    // TBD: test that canmove prevents other actions
            //}

            //[Fact(Skip = "Not implemented")]
            //public void Can_not_attack_before_move_into_captured_territory_has_been_confirmed()
            //{
            //    var defenderIsEliminated = Substitute.For<IBattleResult>();
            //    defenderIsEliminated.IsDefenderDefeated().Returns(true);
            //    //GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //    _battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //        .Returns(defenderIsEliminated);

            //    var sut = Create(_gamePlaySetup);
            //    sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);

            //    sut.AssertCanNotAttack(_currentPlayerRegion, _anotherPlayerRegion);
            //}

            protected override IGameState Create(GameData gameData)
            {
                return new AttackGameState(_gameStateFactory, _battle, _armyDraftCalculator, gameData);
            }
        }

        public class SendInArmiesToOccupyTests : GameStateTestsBase
        {
            protected override IGameState Create(GameData gameData)
            {
                return new SendInArmiesToOccupyGameState(gameData);
            }
        }

        public class GameOverTests : GameStateTestsBase
        {
            //[Fact]
            //public void Is_game_over_when_all_territories_belongs_to_one_player()
            //{
            //    var player = Substitute.For<IPlayer>();
            //    var gamePlaySetup = Make.GamePlaySetup
            //        .WithTerritory(Make.Territory.Player(player).Build())
            //        .WithTerritory(Make.Territory.Player(player).Build())
            //        .Build();

            //    var sut = Create(gamePlaySetup);

            //    sut.IsGameOver().Should().BeTrue();
            //}

            //[Fact]
            //public void Is_not_game_over_when_more_than_one_player_occupies_territories()
            //{
            //    var gamePlaySetup = Make.GamePlaySetup
            //        .WithTerritory(Make.Territory.Build())
            //        .WithTerritory(Make.Territory.Build())
            //        .Build();

            //    var sut = Create(gamePlaySetup);

            //    sut.IsGameOver().Should().BeFalse();
            //}

            protected override IGameState Create(GameData gameData)
            {
                return new GameOverGameState(gameData);
            }
        }

        //public class TurnEndsTests : GameStateTestsBase
        //{
        //[Fact]
        //public void End_turn_passes_turn_to_next_player()
        //{
        //    var nextPlayer = Substitute.For<IPlayer>();
        //    var gameSetup = Make.GamePlaySetup
        //        .WithPlayer(Substitute.For<IPlayer>())
        //        .WithPlayer(nextPlayer)
        //        .Build();

        //    var sut = Create(gameSetup);
        //    sut.EndTurn();

        //    sut.CurrentPlayer.Should().Be(nextPlayer);
        //}

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
        //}
    }
}