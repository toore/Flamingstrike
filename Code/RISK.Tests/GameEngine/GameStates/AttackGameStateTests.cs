using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.Tests.Builders;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace RISK.Tests.GameEngine.GameStates
{
    public class AttackGameStateTests
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IGameRules _gameRules;
        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly IDeck _deck;
        private readonly GameData _gameData;
        private TurnConqueringAchievement _turnConqueringAchievement = TurnConqueringAchievement.NoTerritoryHasBeenConquered;

        public AttackGameStateTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _gameDataFactory = Substitute.For<IGameDataFactory>();
            _attacker = Substitute.For<IAttacker>();
            _fortifier = Substitute.For<IFortifier>();
            _gameRules = Substitute.For<IGameRules>();

            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Make.Player.Build();
            _anotherPlayer = Make.Player.Build();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);
            _anotherTerritory.Player.Returns(_anotherPlayer);

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
        public void Get_number_of_armies_is_zero()
        {
            var sut = Create(_gameData);

            sut.GetNumberOfArmiesToDraft().Should().Be(0);
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
            _attacker.CanAttack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(true);

            var sut = Create(_gameData);

            sut.CanAttack(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack()
        {
            var sut = Create(_gameData);

            sut.CanAttack(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_from_another_players_territory()
        {
            _attacker.CanAttack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _anotherRegion,
                _region).Returns(true);

            var sut = Create(_gameData);

            sut.CanAttack(_anotherRegion, _region).Should().BeFalse();
        }

        [Fact]
        public void Attack_from_another_players_territory_throws()
        {
            var sut = Create(_gameData);
            Action act = () => sut.Attack(_anotherRegion, _region);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Attacks_but_territory_is_defended()
        {
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsNotEliminated);
            var newGameData = Make.GameData.Build();
            _attacker.Attack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(attackOutcome);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                updatedTerritories,
                _deck).Returns(newGameData);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _gameStateConductor.Received().ContinueWithAttackPhase(
                newGameData,
                TurnConqueringAchievement.NoTerritoryHasBeenConquered);
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            var newGameData = Make.GameData.Build();
            _attacker.Attack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(attackOutcome);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                updatedTerritories,
                _deck).Returns(newGameData);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _gameStateConductor.Received().SendArmiesToOccupy(
                newGameData,
                _region,
                _anotherRegion);
        }

        [Fact]
        public void Can_not_send_armies_to_occupy()
        {
            var sut = Create(_gameData);

            sut.CanSendAdditionalArmiesToOccupy().Should().BeFalse();
        }

        [Fact]
        public void Get_number_of_armies_that_can_be_sent_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Send_armies_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.SendAdditionalArmiesToOccupy(1);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_fortify()
        {
            _fortifier.CanFortify(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(true);

            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_fortify_from_another_players_territory()
        {
            _fortifier.CanFortify(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(true);
            _territory.Player.Returns(_anotherPlayer);

            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_fortify()
        {
            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Fortifies()
        {
            var updatedTerritories = new List<ITerritory>();
            var newGameData = Make.GameData.Build();
            _fortifier.Fortify(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion,
                1).Returns(updatedTerritories);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                updatedTerritories,
                _deck).Returns(newGameData);

            var sut = Create(_gameData);
            sut.Fortify(_region, _anotherRegion, 1);

            _gameStateConductor.Received().Fortify(newGameData, _region, _anotherRegion, 1);
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
            var sut = Create(_gameData);
            sut.EndTurn();

            _gameStateConductor.Received().PassTurnToNextPlayer(sut);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            var topDeckCard = Substitute.For<ICard>();
            _deck.Draw().Returns(topDeckCard);
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            var sut = Create(_gameData);
            sut.EndTurn();

            _currentPlayer.Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            _turnConqueringAchievement = TurnConqueringAchievement.NoTerritoryHasBeenConquered;

            var sut = Create(_gameData);
            sut.EndTurn();

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_not_receive_card_after_attack()
        {
            var topDeckCard = Substitute.For<ICard>();
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            _attacker.Attack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(attackOutcome);
            _deck.Draw().Returns(topDeckCard);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_receive_eliminated_players_cards()
        {
            var aCard = Substitute.For<ICard>();
            var aSecondCard = Substitute.For<ICard>();
            var eliminatedPlayersCards = new[] { aCard, aSecondCard };
            _anotherPlayer.AddCard(aCard);
            _anotherPlayer.AddCard(aSecondCard);
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            _attacker.Attack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(attackOutcome);
            _gameRules.IsPlayerEliminated(
                updatedTerritories,
                _anotherPlayer).Returns(true);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _currentPlayer.Cards.ShouldAllBeEquivalentTo(eliminatedPlayersCards, "all cards should be aquired");
            _anotherPlayer.Cards.Should().BeEmpty("all cards should be handed over");
        }

        [Fact]
        public void When_last_defending_player_is_eliminated_the_game_is_over()
        {
            var newGameData = Make.GameData.Build();
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            _attacker.Attack(
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _region,
                _anotherRegion).Returns(attackOutcome);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                updatedTerritories,
                _deck).Returns(newGameData);
            _gameRules.IsGameOver(updatedTerritories).Returns(true);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _gameStateConductor.Received().GameIsOver(newGameData);
        }

        private AttackGameState Create(GameData gameData)
        {
            return new AttackGameState(
                _gameStateConductor,
                _gameDataFactory,
                _attacker,
                _fortifier,
                _gameRules,
                gameData,
                _turnConqueringAchievement);
        }

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
    }
}