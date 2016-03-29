using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
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

            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Substitute.For<IPlayer>();
            _anotherPlayer = Substitute.For<IPlayer>();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);
            _anotherTerritory.Player.Returns(_anotherPlayer);

            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            _gameData = Make.GameData
                .CurrentPlayer(_currentPlayer)
                .WithPlayer(_currentPlayer)
                .WithPlayer(_anotherPlayer)
                .WithTerritory(_territory)
                .WithTerritory(_anotherTerritory)
                .Build();
        }

        [Fact]
        public void Can_not_place_draft_armies()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_region).Should().BeFalse();
        }

        [Fact]
        public void Get_number_of_armies_to_draft_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.GetNumberOfArmiesToDraft();

            act.ShouldThrow<InvalidOperationException>();
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
            var sut = Create(_gameData);

            sut.CanAttack(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack_if_not_enough_attacking_armies()
        {
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(0);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_already_occupied_territory()
        {
            _anotherTerritory.Player.Returns(_currentPlayer);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_territory_without_having_border()
        {
            _region.HasBorder(_anotherRegion).Returns(false);

            var sut = Create(_gameData);

            AssertCanNotAttack(sut, _region, _anotherRegion);
        }

        [Fact]
        public void Can_not_attack_with_another_players_territory()
        {
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

        [Fact]
        public void Attacks_but_territory_is_defended()
        {
            var attackGameState = Substitute.For<IGameState>();
            var updatedAttackingTerritory = Substitute.For<ITerritory>();
            var updatedDefendingTerritory = Substitute.For<ITerritory>();
            var battleResult = Substitute.For<IBattleResult>();
            battleResult.AttackingTerritory.Returns(updatedAttackingTerritory);
            battleResult.DefendingTerritory.Returns(updatedDefendingTerritory);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _gameStateFactory.CreateAttackGameState(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(updatedAttackingTerritory, updatedDefendingTerritory)
                ))
                .Returns(attackGameState);

            var sut = Create(_gameData);
            var result = sut.Attack(_region, _anotherRegion);

            result.Should().Be(attackGameState);
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var sendInArmiesToOccupyGameState = Substitute.For<IGameState>();
            var updatedAttackingTerritory = Substitute.For<ITerritory>();
            var defeatedTerritory = Substitute.For<ITerritory>();
            var battleResult = Substitute.For<IBattleResult>();
            battleResult.AttackingTerritory.Returns(updatedAttackingTerritory);
            battleResult.DefendingTerritory.Returns(defeatedTerritory);
            battleResult.IsDefenderDefeated().Returns(true);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _gameStateFactory.CreateSendInArmiesToOccupyGameState(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(updatedAttackingTerritory, defeatedTerritory)
                ))
                .Returns(sendInArmiesToOccupyGameState);

            var sut = Create(_gameData);
            var result = sut.Attack(_region, _anotherRegion);

            result.Should().Be(sendInArmiesToOccupyGameState);
        }

        [Fact]
        public void Can_not_send_in_armies_to_occupy()
        {
            var sut = Create(_gameData);

            sut.CanSendArmiesToOccupy().Should().BeFalse();
        }

        [Fact]
        public void Get_number_of_armies_that_can_be_sent_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.GetNumberOfArmiesThatCanBeSentToOccupy();

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Send_armies_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.SendArmiesToOccupy(1);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_fortify()
        {
            _anotherTerritory.Player.Returns(_currentPlayer);

            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Fortifies()
        {
            var fortifyGameState = Substitute.For<IGameState>();
            _anotherTerritory.Player.Returns(_currentPlayer);
            _gameStateFactory.CreateFortifyState(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(_territory, _anotherTerritory)
                ), 
                _region, 
                _anotherRegion, 
                1)
                .Returns(fortifyGameState);

            var sut = Create(_gameData);
            var actual = sut.Fortify(_region, _anotherRegion, 1);

            actual.Should().Be(fortifyGameState);
        }

        [Fact]
        public void Can_not_fortify_non_bordering_regions()
        {
            _anotherTerritory.Player.Returns(_currentPlayer);
            _region.HasBorder(_anotherRegion).Returns(false);

            var sut = Create(_gameData);

            AssertCanNotFortify(sut, _region, _anotherRegion, 1);
        }

        [Fact]
        public void Can_not_fortify_from_another_players_territory()
        {
            _territory.Player.Returns(_anotherPlayer);
            _anotherRegion.HasBorder(_region).Returns(true);

            var sut = Create(_gameData);

            AssertCanNotFortify(sut, _anotherRegion, _region, 1);
        }

        [Fact]
        public void Can_not_fortify_to_another_players_territory()
        {
            _anotherTerritory.Player.Returns(_currentPlayer);

            var sut = Create(_gameData);

            AssertCanNotFortify(sut, _anotherRegion, _region, 1);
        }

        private static void AssertCanNotFortify(IGameState gameState, IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            Action act = () => gameState.Fortify(sourceRegion, destinationRegion, armies);

            gameState.CanFortify(sourceRegion, destinationRegion).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_end_turn()
        {
            var sut = Create(_gameData);

            sut.CanEndTurn().Should().BeTrue();
        }



        

        protected override IGameState Create(GameData gameData)
        {
            return new AttackGameState(_gameStateFactory, _battle, gameData);
        }
    }
}