using System;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class AttackGameStateTests
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IBattle _battle;
        private readonly IAttackPhaseRules _attackPhaseRules;
        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IInGamePlayer _currentPlayer;
        private readonly IInGamePlayer _anotherPlayer;
        private readonly IDeck _deck;
        private readonly GameData _gameData;

        public AttackGameStateTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _gameDataFactory = Substitute.For<IGameDataFactory>();
            _battle = Substitute.For<IBattle>();
            _attackPhaseRules = Substitute.For<IAttackPhaseRules>();

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
            Action act = () => gameState.Attack(attackingRegion, defendingRegion);

            gameState.CanAttack(attackingRegion, defendingRegion).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Attacks_but_territory_is_defended()
        {
            var updatedAttackingTerritory = Substitute.For<ITerritory>();
            var updatedDefendingTerritory = Substitute.For<ITerritory>();
            var battleResult = Substitute.For<IBattleOutcome>();
            var newGameData = Make.GameData.Build();
            battleResult.UpdatedAttackingTerritory.Returns(updatedAttackingTerritory);
            battleResult.UpdatedDefendingTerritory.Returns(updatedDefendingTerritory);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(updatedAttackingTerritory, updatedDefendingTerritory),
                _deck)
                .Returns(newGameData);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _gameStateConductor.Received().ContinueWithAttackPhase(
                newGameData,
                ConqueringAchievement.DoNotAwardCardAtEndOfTurn);
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var updatedAttackingTerritory = Substitute.For<ITerritory>();
            var defeatedTerritory = Substitute.For<ITerritory>();
            var battleResult = Substitute.For<IBattleOutcome>();
            var newGameData = Make.GameData.Build();
            battleResult.UpdatedAttackingTerritory.Returns(updatedAttackingTerritory);
            battleResult.UpdatedDefendingTerritory.Returns(defeatedTerritory);
            battleResult.IsDefenderDefeated().Returns(true);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(updatedAttackingTerritory, defeatedTerritory),
                _deck)
                .Returns(newGameData);

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
            _anotherTerritory.Player.Returns(_currentPlayer);

            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Fortifies()
        {
            var newGameData = Make.GameData.Build();
            _anotherTerritory.Player.Returns(_currentPlayer);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(_territory, _anotherTerritory),
                _deck)
                .Returns(newGameData);

            var sut = Create(_gameData);
            sut.Fortify(_region, _anotherRegion, 1);

            _gameStateConductor.Received().Fortify(newGameData, _region, _anotherRegion, 1);
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
            var sut = Create(_gameData);
            sut.EndTurn();

            _gameStateConductor.Received().PassTurnToNextPlayer(sut);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            var topDeckCard = Substitute.For<ICard>();
            var battleResult = Substitute.For<IBattleOutcome>();
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
            var battleResult = Substitute.For<IBattleOutcome>();
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
            var battleResult = Substitute.For<IBattleOutcome>();
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
            var aCard = Substitute.For<ICard>();
            var aSecondCard = Substitute.For<ICard>();
            var eliminatedPlayersCards = new[] { aCard, aSecondCard };
            var battleResult = Substitute.For<IBattleOutcome>();
            _anotherPlayer.AddCard(aCard);
            _anotherPlayer.AddCard(aSecondCard);
            battleResult.IsDefenderDefeated().Returns(true);
            battleResult.UpdatedDefendingTerritory.Returns(Substitute.For<ITerritory>());
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _currentPlayer.Cards.ShouldAllBeEquivalentTo(eliminatedPlayersCards, "all cards should be aquired");
            _anotherPlayer.Cards.Should().BeEmpty("all cards should be handed over");
        }

        [Fact]
        public void When_last_defending_player_is_eliminated_the_game_is_over()
        {
            var battleResult = Substitute.For<IBattleOutcome>();
            var updatedTerritory = Substitute.For<ITerritory>();
            var anotherUpdatedTerritory = Substitute.For<ITerritory>();
            var newGameData = Make.GameData.Build();
            battleResult.IsDefenderDefeated().Returns(true);
            updatedTerritory.Player.Returns(_currentPlayer);
            anotherUpdatedTerritory.Player.Returns(_currentPlayer);
            battleResult.UpdatedAttackingTerritory.Returns(updatedTerritory);
            battleResult.UpdatedDefendingTerritory.Returns(anotherUpdatedTerritory);
            _battle.Attack(_territory, _anotherTerritory).Returns(battleResult);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(updatedTerritory, anotherUpdatedTerritory),
                _deck)
                .Returns(newGameData);

            var sut = Create(_gameData);
            sut.Attack(_region, _anotherRegion);

            _gameStateConductor.Received().GameIsOver(newGameData);
        }

        private IGameState Create(GameData gameData)
        {
            return new AttackGameState(_gameStateConductor, _gameDataFactory, _battle, _attackPhaseRules, gameData);
        }

        [Fact]
        public void Gets_current_player()
        {
            var currentPlayer = Substitute.For<IInGamePlayer>();
            var gameData = Make.GameData
                .CurrentPlayer(currentPlayer)
                .Build();

            var sut = Create(gameData);

            sut.CurrentPlayer.Should().Be(currentPlayer);
        }
    }
}