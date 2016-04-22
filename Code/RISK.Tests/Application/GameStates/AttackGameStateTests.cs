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
        private readonly IDeck _deck;
        private readonly GameData _gameData;

        public AttackGameStateTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _battle = Substitute.For<IBattle>();

            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Make.Player.Build();
            _anotherPlayer = Substitute.For<IPlayer>();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);
            _anotherTerritory.Player.Returns(_anotherPlayer);

            _region.HasBorder(_anotherRegion).Returns(true);
            _territory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            _deck = Substitute.For<IDeck>();

            _gameData = Make.GameData
                .CurrentPlayer(_currentPlayer)
                .WithPlayer(_currentPlayer)
                .WithPlayer(_anotherPlayer)
                .WithTerritory(_territory)
                .WithTerritory(_anotherTerritory)
                .WithDeck(_deck)
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

        [Fact]
        public void End_turn_passes_turn_to_next_player()
        {
            var nextGameState = Substitute.For<IGameState>();
            var sut = Create(_gameData);
            _gameStateFactory.CreateNextTurnGameState(sut).Returns(nextGameState);

            var actualGameState = sut.EndTurn();

            actualGameState.Should().Be(nextGameState);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            var topDeckCard = Substitute.For<ICard>();
            var battleResult = Substitute.For<IBattleResult>();
            battleResult.IsDefenderDefeated().Returns(true);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _deck.Draw().Returns(topDeckCard);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);
            sut.EndTurn();

            _currentPlayer.Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_should_not_receive_card_after_attack()
        {
            var topDeckCard = Substitute.For<ICard>();
            var battleResult = Substitute.For<IBattleResult>();
            battleResult.IsDefenderDefeated().Returns(true);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _deck.Draw().Returns(topDeckCard);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            var battleResult = Substitute.For<IBattleResult>();
            battleResult.IsDefenderDefeated().Returns(false);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);
            sut.EndTurn();

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_receive_eliminated_players_cards()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Player_should_receive_eliminated_players_cards_and_draft_armies_immediately()
        {
            throw new NotImplementedException();
        }

        protected override IGameState Create(GameData gameData)
        {
            return new AttackGameState(_gameStateFactory, _battle, gameData);
        }
    }
}