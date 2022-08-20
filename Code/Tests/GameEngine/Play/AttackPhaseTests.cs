using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.GameEngine.Play
{
    public abstract class AttackPhaseTests
    {
        private readonly IDeck _deck;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IAttackService _attackService;
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private readonly IWorldMap _worldMap;
        private readonly Region _region;
        private readonly Region _anotherRegion;
        private readonly PlayerName _currentPlayerName;
        private readonly PlayerName _anotherPlayerName;
        private readonly Player _currentPlayer;
        private IPlayer _anotherPlayer;
        private ITerritory _territory;
        private Territory _anotherTerritory;
        private ConqueringAchievement _conqueringAchievement = ConqueringAchievement.NoTerritoryHasBeenConquered;

        protected AttackPhaseTests()
        {
            _deck = Substitute.For<IDeck>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _attackService = Substitute.For<IAttackService>();
            _playerEliminationRules = Substitute.For<IPlayerEliminationRules>();

            _worldMap = new WorldMapFactory().Create();
            _region = Region.NorthAfrica;
            _anotherRegion = Region.Brazil;

            _currentPlayerName = new PlayerName("current player");
            _anotherPlayerName = new PlayerName("another player");

            _currentPlayer = new PlayerBuilder().Name(_currentPlayerName).Build();
        }

        private AttackPhase Sut => new AttackPhase(
            _gamePhaseConductor,
            _currentPlayerName,
            new[] { _territory, _anotherTerritory },
            new[] { _currentPlayer, _anotherPlayer },
            _deck,
            _conqueringAchievement,
            _attackService,
            _playerEliminationRules,
            _worldMap);

        public class Attacking : AttackPhaseTests
        {
            private readonly ICard _card;
            private readonly ICard _anotherCard;

            public Attacking()
            {
                _card = Substitute.For<ICard>();
                _anotherCard = Substitute.For<ICard>();

                _anotherPlayer = new PlayerBuilder().Name(_anotherPlayerName)
                    .AddCard(_card)
                    .AddCard(_anotherCard).Build();

                _territory = new TerritoryBuilder().Region(_region).Player(_currentPlayerName).Armies(2).Build();
                _anotherTerritory = new TerritoryBuilder().Region(_anotherRegion).Player(_anotherPlayerName).Armies(1).Build();
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
                _attackService.Attack(_territory, _anotherTerritory).Returns(DefendingArmyStatus.IsAlive);

                Sut.Attack(_region, _anotherRegion);

                _gamePhaseConductor.Received().ContinueWithAttackPhase(ConqueringAchievement.NoTerritoryHasBeenConquered);
            }

            [Fact]
            public void Attacks_and_defeats_defender()
            {
                _attackService.Attack(_territory, _anotherTerritory).Returns(DefendingArmyStatus.IsEliminated);

                Sut.Attack(_region, _anotherRegion);

                _gamePhaseConductor.Received().SendArmiesToOccupy(_region, _anotherRegion);
            }

            [Fact]
            public void Fortify_from_another_players_territory_throws()
            {
                Action act = () => Sut.Fortify(_anotherRegion, _region, 1);

                act.Should().Throw<InvalidOperationException>();
            }

            [Fact]
            public void End_turn_passes_turn_to_next_player()
            {
                Sut.EndTurn();

                _gamePhaseConductor.Received().PassTurnToNextPlayer();
            }

            [Fact]
            public void Player_should_be_awarded_card_when_turn_ends()
            {
                var topDeckCard = Substitute.For<ICard>();
                _deck.DrawCard().Returns(topDeckCard);
                _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

                Sut.EndTurn();

                _currentPlayer.Cards.Should().BeEquivalentTo(new[] { topDeckCard });
            }

            [Fact]
            public void Player_should_not_be_awarded_card_when_turn_ends()
            {
                Sut.EndTurn();

                _currentPlayer.Cards.Should().BeEmpty();
            }

            [Fact]
            public void Player_should_not_be_awarded_card_after_attack()
            {
                _attackService.Attack(_territory, _anotherTerritory).Returns(DefendingArmyStatus.IsEliminated);

                Sut.Attack(_region, _anotherRegion);

                _currentPlayer.Cards.Should().BeEmpty();
            }

            [Fact]
            public void Player_should_receive_eliminated_players_cards()
            {
                _attackService.Attack(_territory, _anotherTerritory).Returns(DefendingArmyStatus.IsEliminated);
                _playerEliminationRules.IsPlayerEliminated(Arg.Is<IEnumerable<ITerritory>>(x => x.SequenceEqual(new[] { _territory, _anotherTerritory })), _anotherPlayerName).Returns(true);

                Sut.Attack(_region, _anotherRegion);

                _currentPlayer.Cards.Should().BeEquivalentTo(new[] { _card, _anotherCard });
                _anotherPlayer.Cards.Should().BeEmpty();
            }

            [Fact]
            public void When_last_defending_player_is_eliminated_the_game_is_over()
            {
                _attackService.Attack(_territory, _anotherTerritory).Returns(DefendingArmyStatus.IsEliminated);
                _playerEliminationRules.IsOnePlayerLeftInTheGame(Arg.Is<IEnumerable<ITerritory>>(x => x.SequenceEqual(new[] { _territory, _anotherTerritory }))).Returns(true);

                Sut.Attack(_region, _anotherRegion);

                _gamePhaseConductor.Received().PlayerIsTheWinner(_currentPlayerName);
            }
        }

        public class Fortification : AttackPhaseTests
        {
            public Fortification()
            {
                _anotherPlayer = new PlayerBuilder().Name(_anotherPlayerName).Build();

                _territory = new TerritoryBuilder().Region(_region).Player(_currentPlayerName).Armies(6).Build();
                _anotherTerritory = new TerritoryBuilder().Region(_anotherRegion).Player(_currentPlayerName).Armies(1).Build();
            }

            [Fact]
            public void Fortifies()
            {
                Sut.Fortify(_region, _anotherRegion, 2);

                _territory.Armies.Should().Be(4);
                _anotherTerritory.Armies.Should().Be(3);
            }

            [Fact]
            public void Fortifies_and_waits_for_turn_to_end()
            {
                Sut.Fortify(_region, _anotherRegion, 2);

                _gamePhaseConductor.Received().WaitForTurnToEnd();
            }

            [Fact]
            public void Player_is_awarded_card_after_fortification()
            {
                var topDeckCard = Substitute.For<ICard>();
                _deck.DrawCard().Returns(topDeckCard);
                _conqueringAchievement = ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory;

                Sut.Fortify(_region, _anotherRegion, 1);

                _currentPlayer.Cards.Should().BeEquivalentTo(new[] { topDeckCard });
            }

            [Fact]
            public void Player_is_not_awarded_card_after_fortification()
            {
                Sut.Fortify(_region, _anotherRegion, 1);

                _currentPlayer.Cards.Should().BeEmpty();
            }
        }
    }
}