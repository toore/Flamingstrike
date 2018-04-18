using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using Xunit;

namespace Tests.GameEngine.Play
{
    public class AttackPhaseTests
    {
        private readonly GameData _gameData;
        private readonly PlayerName _currentPlayerName;
        private readonly PlayerName _anotherPlayerName;
        private readonly IPlayer _anotherPlayer;
        private readonly IDeck _deck;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttackService _attackService;
        private readonly IFortifier _fortifier;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.NoTerritoryHasBeenConquered;
        private readonly ITerritory _territory;
        private readonly Region _region;
        private readonly Region _anotherRegion;
        private readonly IWorldMap _worldMap;
        private readonly Player _currentPlayer;

        public AttackPhaseTests()
        {
            _deck = Substitute.For<IDeck>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _attackService = Substitute.For<IAttackService>();
            _fortifier = Substitute.For<IFortifier>();
            _playerEliminationRules = Substitute.For<IPlayerEliminationRules>();

            _worldMap = new WorldMapFactory().Create();
            _region = Region.Alaska;
            _anotherRegion = Region.Brazil;
            _territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();

            _currentPlayerName = new PlayerName("current player");
            _anotherPlayerName = new PlayerName("another player");
            _anotherPlayer = Substitute.For<IPlayer>();
            _anotherPlayer.Name.Returns(_anotherPlayerName);

            _territory.Region.Returns(_region);
            _territory.Name.Returns(_currentPlayerName);
            anotherTerritory.Region.Returns(_anotherRegion);
            anotherTerritory.Name.Returns(_anotherPlayerName);

            _currentPlayer = new PlayerBuilder().Player(_currentPlayerName).Build();
            _gameData = new GameDataBuilder()
                .Territories(_territory, anotherTerritory)
                .AddPlayer(_currentPlayer)
                .AddPlayer(_anotherPlayer)
                .CurrentPlayer(_currentPlayerName)
                .Deck(_deck)
                .Build();
        }

        private AttackPhase Sut => new AttackPhase(
            _gamePhaseConductor,
            _currentPlayerName,
            _gameData.Territories,
            _gameData.Players,
            _gameData.Deck,
            _conqueringAchievement,
            _attackService,
            _fortifier,
            _playerEliminationRules,
            _worldMap);

        [Fact]
        public void Can_attack()
        {
            //_attacker.CanAttack(
            //        _gameData.Territories,
            //        _region,
            //        _anotherRegion)
            //    .Returns(true);

            //Sut.CanAttack(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack()
        {
            //Sut.CanAttack(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_from_another_players_territory()
        {
            //Sut.CanAttack(_anotherRegion, _region).Should().BeFalse();
        }

        [Fact]
        public void Attack_from_another_players_territory_throws()
        {
            Action act = () => Sut.Attack(_anotherRegion, _region);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void Attacks_but_territory_is_defended()
        {
            var expectedUpdatedTerritories = new List<ITerritory>();
            //var attackOutcome = new AttackOutcome(expectedUpdatedTerritories, DefendingArmy.IsAlive);
            //_attackService.Attack(
            //        _region,
            //        _anotherRegion)
            //    .Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(ConqueringAchievement.NoTerritoryHasBeenConquered, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Attacks_and_defeats_defender()
        {
            var expectedUpdatedTerritories = new List<ITerritory>
                {
                    new TerritoryBuilder().Region(_region).Armies(2).Build(),
                    new TerritoryBuilder().Region(_anotherRegion).Build()
                };

            //var attackOutcome = new AttackOutcome(expectedUpdatedTerritories, DefendingArmy.IsEliminated);
            //_attackService.Attack(
            //        _region,
            //        _anotherRegion)
            //    .Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().SendArmiesToOccupy(_region, _anotherRegion, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Can_fortify()
        {
            //_fortifier.CanFortify(
            //        _gameData.Territories,
            //        _region,
            //        _anotherRegion)
            //    .Returns(true);

            //Sut.CanFortify(_region, _anotherRegion).Should().BeTrue();
        }

        [Fact]
        public void Can_not_fortify()
        {
            //Sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Can_not_fortify_from_another_players_territory()
        {
            //Sut.CanFortify(_anotherRegion, _region).Should().BeFalse();
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
            _deck.DrawCard().Returns(topDeckCard);
            _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.Fortify(_region, _anotherRegion, 1);

            updatedGameData.GetCurrentPlayer().Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_is_not_awarded_card_after_fortification()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.WaitForTurnToEnd(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.Fortify(_region, _anotherRegion, 1);

            updatedGameData.GetCurrentPlayer().Cards.Should().BeEmpty();
        }

        [Fact]
        public void End_turn_passes_turn_to_next_player()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.EndTurn();

            _gameData.Should().BeEquivalentTo(updatedGameData);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));
            var topDeckCard = Substitute.For<ICard>();
            _deck.DrawCard().Returns(topDeckCard);
            _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

            Sut.EndTurn();

            updatedGameData.GetCurrentPlayer().Cards.Should().BeEquivalentTo(topDeckCard);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.PassTurnToNextPlayer(Arg.Do<GameData>(x => updatedGameData = x));

            Sut.EndTurn();

            updatedGameData.GetCurrentPlayer().Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_not_receive_card_after_attack()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.ContinueWithAttackPhase(ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory, Arg.Do<GameData>(x => updatedGameData = x));
            var updatedTerritories = new List<ITerritory> { _territory };
            //var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            //_attackService.Attack(
            //        _region,
            //        _anotherRegion)
            //    .Returns(attackOutcome);

            Sut.Attack(_region, _anotherRegion);

            updatedGameData.GetCurrentPlayer().Cards.Should().BeEmpty();
        }

        [Fact]
        public void Player_should_receive_eliminated_players_cards()
        {
            GameData updatedGameData = null;
            _gamePhaseConductor.ContinueWithAttackPhase(
                ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                Arg.Do<GameData>(x => updatedGameData = x));
            var card = Substitute.For<ICard>();
            var anotherCard = Substitute.For<ICard>();
            _anotherPlayer
                .When(x => x.EliminatedBy(_currentPlayer))
                .Do(x => _currentPlayer.AddCards(new[] { card, anotherCard }));
            var updatedTerritories = new List<ITerritory> { _territory };
            //var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            //_attackService.Attack(
            //        _region,
            //        _anotherRegion)
            //    .Returns(attackOutcome);
            _playerEliminationRules.IsPlayerEliminated(
                    updatedTerritories,
                    _anotherPlayerName)
                .Returns(true);

            Sut.Attack(_region, _anotherRegion);

            updatedGameData.GetCurrentPlayer().Cards
                .Should().BeEquivalentTo(new[] { card, anotherCard }, config => config.WithStrictOrdering(), "all cards should be aquired from eliminated player");
            updatedGameData.Players.Single(x => x.Name == _anotherPlayerName).Cards
                .Should().BeEmpty("all cards should be handed over");
        }

        [Fact]
        public void When_last_defending_player_is_eliminated_the_game_is_over()
        {
            var updatedTerritories = new List<ITerritory>();
            //var attackOutcome = new AttackOutcome(updatedTerritories, DefendingArmy.IsEliminated);
            //_attackService.Attack(
            //        _region,
            //        _anotherRegion)
            //    .Returns(attackOutcome);
            _playerEliminationRules.IsOnePlayerLeftInTheGame(updatedTerritories).Returns(true);

            Sut.Attack(_region, _anotherRegion);

            _gamePhaseConductor.Received().PlayerIsTheWinner(_currentPlayerName);
        }
    }

    public static class GameDataExtensions
    {
        public static IPlayer GetCurrentPlayer(this GameData gameData)
        {
            return gameData.Players.Single(x => x.Name == gameData.CurrentPlayerName);
        }
    }
}