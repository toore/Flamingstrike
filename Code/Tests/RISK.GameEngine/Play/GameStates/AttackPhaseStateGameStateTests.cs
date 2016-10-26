using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Attacking;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using Tests.RISK.GameEngine.Builders;
using Xunit;

namespace Tests.RISK.GameEngine.Play.GameStates
{
    public class AttackPhaseStateGameStateTests
    {
        private readonly GameData _gameData;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly IPlayerGameData _anotherPlayerGameData;
        private readonly IDeck _deck;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private TurnConqueringAchievement _turnConqueringAchievement = TurnConqueringAchievement.NoTerritoryHasBeenConquered;
        private readonly ITerritory _territory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;

        public AttackPhaseStateGameStateTests()
        {
            _deck = Substitute.For<IDeck>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _attacker = Substitute.For<IAttacker>();
            _fortifier = Substitute.For<IFortifier>();
            _playerEliminationRules = Substitute.For<IPlayerEliminationRules>();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();

            _currentPlayer = Substitute.For<IPlayer>();
            _anotherPlayer = Substitute.For<IPlayer>();
            _anotherPlayerGameData = Substitute.For<IPlayerGameData>();

            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            anotherTerritory.Region.Returns(_anotherRegion);
            anotherTerritory.Player.Returns(_anotherPlayer);

            _gameData = Make.GameData
                .Territories(_territory, anotherTerritory)
                .AddPlayer(Make.PlayerGameData.Player(_currentPlayer).Build())
                .AddPlayer(_anotherPlayerGameData)
                .CurrentPlayer(_currentPlayer)
                .Deck(_deck)
                .Build();
        }

        private AttackPhaseStateGameState Sut => new AttackPhaseStateGameState(
            _gameData,
            _gamePhaseConductor,
            _attacker,
            _fortifier,
            _playerEliminationRules,
            _turnConqueringAchievement);

        [Fact]
        public void Can_attack()
        {
            _attacker.CanAttack(
                _gameData.Territories,
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
            var expectedUpdatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(expectedUpdatedTerritories, DefendingArmyAvailability.Exists);
            _attacker.Attack(
                _gameData.Territories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(TurnConqueringAchievement.NoTerritoryHasBeenConquered, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var expectedUpdatedTerritories = new List<ITerritory>
                {
                    Make.Territory.Region(_region).Armies(2).Build(),
                    Make.Territory.Region(_anotherRegion).Build()
                };

            var attackOutcome = new AttackOutcome(expectedUpdatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _gameData.Territories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().SendArmiesToOccupy(_region, _anotherRegion, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Can_fortify()
        {
            _fortifier.CanFortify(
                _gameData.Territories,
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
            var expectedUpdatedTerritories = new List<ITerritory>();
            _fortifier.Fortify(_gameData.Territories, _region, _anotherRegion, 1).Returns(expectedUpdatedTerritories);

            Sut.Fortify(_region, _anotherRegion, 1);

            _gamePhaseConductor.Received().WaitForTurnToEnd(Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Player_is_awarded_card_after_fortification()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.WaitForTurnToEnd(Arg.Do<GameData>(x => updatedGameData = x));
            var topDeckCard = Substitute.For<ICard>();
            _deck.DrawCard().Returns(new CardDrawnAndRestOfDeck(topDeckCard, Make.Deck.Build()));
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.Fortify(_region, _anotherRegion, 1);

            updatedGameData.GetCurrentPlayerGameData().Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_is_not_awarded_card_after_fortification()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.WaitForTurnToEnd(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.Fortify(_region, _anotherRegion, 1);

            updatedGameData.GetCurrentPlayerGameData().Cards.Should().BeEmpty();
        }

        [Fact]
        public void End_turn_passes_turn_to_next_player()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.EndTurn();

            _gameData.ShouldBeEquivalentTo(updatedGameData);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));
            var topDeckCard = Substitute.For<ICard>();
            _deck.DrawCard().Returns(new CardDrawnAndRestOfDeck(topDeckCard, null));
            _turnConqueringAchievement = TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.EndTurn();

            updatedGameData.GetCurrentPlayerGameData().Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.EndTurn();

            updatedGameData.GetCurrentPlayerGameData().Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_not_receive_card_after_attack()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.ContinueWithAttackPhase(TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory, Arg.Do<GameData>(x => updatedGameData = x));
            var updatedTerritories = new List<ITerritory> { _territory };
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _gameData.Territories,
                _region,
                _anotherRegion).Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            updatedGameData.GetCurrentPlayerGameData().Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_receive_eliminated_players_cards()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.ContinueWithAttackPhase(
                TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                Arg.Do<GameData>(x => updatedGameData = x));
            var aCard = Substitute.For<ICard>();
            var aSecondCard = Substitute.For<ICard>();
            _anotherPlayerGameData.Player.Returns(_anotherPlayer);
            _anotherPlayerGameData.Cards.Returns(new[] { aCard, aSecondCard });
            var updatedTerritories = new List<ITerritory> { _territory };
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _gameData.Territories,
                _region,
                _anotherRegion).Returns(attackOutcome);
            _playerEliminationRules.IsPlayerEliminated(
                updatedTerritories,
                _anotherPlayer).Returns(true);

            Sut.Attack(_region, _anotherRegion);

            updatedGameData.GetCurrentPlayerGameData().Cards.ShouldAllBeEquivalentTo(new[] { aCard, aSecondCard }, "all cards should be aquired from eliminated player");
            updatedGameData.PlayerGameDatas.Single(x => x.Player == _anotherPlayer).Cards.Should().BeEmpty("all cards should be handed over");
        }

        [Fact]
        public void When_last_defending_player_is_eliminated_the_game_is_over()
        {
            var updatedTerritories = new List<ITerritory>();
            var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmyAvailability.IsEliminated);
            _attacker.Attack(
                _gameData.Territories,
                _region,
                _anotherRegion).Returns(attackOutcome);
            _playerEliminationRules.IsOnlyOnePlayerLeftInTheGame(updatedTerritories).Returns(true);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().PlayerIsTheWinner(_currentPlayer);
        }
    }
}