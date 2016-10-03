using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using Tests.Builders;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace Tests.RISK.GameEngine.Play.GameStates
{
    public class AttackPhaseStateGameStateTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IReadOnlyList<IPlayer> _players;
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IDeck _deck;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private TurnConqueringAchievement _turnConqueringAchievement = TurnConqueringAchievement.NoTerritoryHasBeenConquered;
        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IPlayer _anotherPlayer;
        private readonly IReadOnlyList<ITerritory> _territories;

        public AttackPhaseStateGameStateTests()
        {
            _currentPlayer = Make.Player.Build();
            _territoriesContext = new TerritoriesContext();
            _deck = Substitute.For<IDeck>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _attacker = Substitute.For<IAttacker>();
            _fortifier = Substitute.For<IFortifier>();
            _playerEliminationRules = Substitute.For<IPlayerEliminationRules>();

            _anotherPlayer = Make.Player.Build();

            _players = new List<IPlayer> { _currentPlayer, _anotherPlayer };

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();

            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);
            _anotherTerritory.Player.Returns(_anotherPlayer);

            _territories = new List<ITerritory> { _territory, _anotherTerritory };
            _territoriesContext.Set(_territories);
        }

        private AttackPhaseStateGameState Sut => new AttackPhaseStateGameState(
            _currentPlayer,
            _players,
            _territoriesContext,
            _deck,
            _gamePhaseConductor,
            _attacker,
            _fortifier,
            _playerEliminationRules,
            _turnConqueringAchievement);

        [Fact]
        public void Can_attack()
        {
            _attacker.CanAttack(
                _territories,
                _region,
                _anotherRegion).Returns(true);

            Sut.CanAttack(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack()
        {
            Sut.CanAttack(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_from_another_players_territory()
        {
            Sut.CanAttack(_anotherRegion, _region).Should().BeFalse();
        }

        [Fact]
        public void Attack_from_another_players_territory_throws()
        {
            Action act = () => Sut.Attack(_anotherRegion, _region);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Attacks_but_territory_is_defended()
        {
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.Exists);
            _attacker.Attack(
                _territories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(TurnConqueringAchievement.NoTerritoryHasBeenConquered);
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var originalTerritories = new List<ITerritory>
                {
                    Make.Territory.Region(_region).Player(_currentPlayer).Build(),
                    Make.Territory.Region(_anotherRegion).Build()
                };
            var updatedTerritories = new List<ITerritory>
                {
                    Make.Territory.Region(_region).Armies(2).Build(),
                    Make.Territory.Region(_anotherRegion).Build()
                };
            _territoriesContext.Set(originalTerritories);
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                originalTerritories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().SendArmiesToOccupy(_region, _anotherRegion);
        }

        [Fact]
        public void Can_fortify()
        {
            _fortifier.CanFortify(
                _territories,
                _region,
                _anotherRegion).Returns(true);

            Sut.CanFortify(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_fortify()
        {
            Sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_fortify_from_another_players_territory()
        {
            Sut.CanFortify(_anotherRegion, _region).Should().BeFalse();
        }

        [Fact]
        public void Fortifies()
        {
            var updatedTerritories = new List<ITerritory>();
            _fortifier.Fortify(_territories, _region, _anotherRegion, 1).Returns(updatedTerritories);

            Sut.Fortify(_region, _anotherRegion, 1);

            _territoriesContext.Territories.Should().BeSameAs(updatedTerritories);
            _gamePhaseConductor.Received().WaitForTurnToEnd();
        }

        [Fact]
        public void Player_is_awarded_card_after_fortification()
        {
            var topDeckCard = Substitute.For<ICard>();
            _deck.Draw().Returns(topDeckCard);
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.Fortify(_region, _anotherRegion, 1);

            _currentPlayer.Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_is_not_awarded_card_after_fortification()
        {
            Sut.Fortify(_region, _anotherRegion, 1);

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void End_turn_passes_turn_to_next_player()
        {
            Sut.EndTurn();

            _gamePhaseConductor.Received().PassTurnToNextPlayer();
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            var topDeckCard = Substitute.For<ICard>();
            _deck.Draw().Returns(topDeckCard);
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.EndTurn();

            _currentPlayer.Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            Sut.EndTurn();

            _currentPlayer.Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_not_receive_card_after_attack()
        {
            var updatedTerritories = new List<ITerritory> { _territory };
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _territories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

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
            var updatedTerritories = new List<ITerritory> { _territory };
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _territories,
                _region,
                _anotherRegion).Returns(attackOutcome);
            _playerEliminationRules.IsPlayerEliminated(
                updatedTerritories,
                _anotherPlayer).Returns(true);

            Sut.Attack(_region, _anotherRegion);

            _currentPlayer.Cards.ShouldAllBeEquivalentTo(eliminatedPlayersCards, "all cards should be aquired");
            _anotherPlayer.Cards.Should().BeEmpty("all cards should be handed over");
        }

        [Fact]
        public void When_last_defending_player_is_eliminated_the_game_is_over()
        {
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _territories,
                _region,
                _anotherRegion).Returns(attackOutcome);
            _playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(updatedTerritories).Returns(true);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().PlayerIsTheWinner(_currentPlayer);
        }
    }
}