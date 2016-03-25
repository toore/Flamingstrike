using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
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

    public class AttackGameStateTests : GameStateTestsBase
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IBattle _battle;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly ITerritoryUpdater _territoryUpdater;

        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly GameData _gameData;

        public AttackGameStateTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _battle = Substitute.For<IBattle>();
            _armyDraftCalculator = Substitute.For<IArmyDraftCalculator>();
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

        [Fact]
        public void Get_number_of_armies_to_draft_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.GetNumberOfArmiesToDraft();

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_not_place_draft_armies()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_region).Should().BeFalse();
        }

        [Fact]
        public void Place_draft_armies_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.PlaceDraftArmies(_region, 1);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_attack()
        {
            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            var sut = Create(_gameData);

            sut.CanAttack(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack_if_not_enough_attacking_armies()
        {
            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(0);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_already_occupied_territory()
        {
            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);
            _anotherTerritory.Player.Returns(_currentPlayer);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_territory_without_having_border()
        {
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_with_another_players_territory()
        {
            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);
            _territory.Player.Returns(_anotherPlayer);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        private static void AssertCanNotAttack(IGameState gameState, IRegion attackingRegion, IRegion defendingRegion)
        {
            Action attackMethod = () => gameState.Attack(attackingRegion, defendingRegion);

            gameState.CanAttack(attackingRegion, defendingRegion).Should().BeFalse();
            attackMethod.ShouldThrow<InvalidOperationException>();
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
}