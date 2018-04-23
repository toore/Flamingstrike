using System;
using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.GameEngine.Play
{
    public class DraftArmiesPhaseTests
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly Region _region;
        private readonly Region _anotherRegion;
        private readonly int _numberOfArmiesToDraft;
        private readonly PlayerName _currentPlayerName;
        private readonly Territory _territory;
        private readonly Territory _anotherTerritory;
        private readonly Player _player;

        public DraftArmiesPhaseTests()
        {
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();

            _currentPlayerName = new PlayerName("current player");

            _player = new PlayerBuilder().Name(_currentPlayerName).Build();

            _region = Region.Brazil;
            _anotherRegion = Region.NorthAfrica;

            _territory = new TerritoryBuilder().Region(_region).Player(_currentPlayerName).Armies(2).Build();
            _anotherTerritory = new TerritoryBuilder().Region(_anotherRegion).Build();

            _numberOfArmiesToDraft = 3;
        }

        private DraftArmiesPhase Sut => new DraftArmiesPhase(
            _gamePhaseConductor,
            _currentPlayerName,
            new[] { _territory, _anotherTerritory },
            new[] { _player, new PlayerBuilder().Name(_anotherTerritory.PlayerName).Build() },
            new Deck(new Stack<ICard>()),
            _numberOfArmiesToDraft);

        [Fact]
        public void Place_draft_armies_for_occupied_territory()
        {
            Sut.PlaceDraftArmies(_region, 1);

            _territory.Armies.Should().Be(3);
        }

        [Fact]
        public void Place_draft_armies_throws_for_another_territory()
        {
            Action act = () => Sut.PlaceDraftArmies(_anotherRegion, 1);

            act.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void After_placing_draft_armies_continues_to_draft_armies()
        {
            Sut.PlaceDraftArmies(_region, 1);

            _gamePhaseConductor.Received().ContinueToDraftArmies(2);
        }

        [Fact]
        public void After_placing_all_draft_armies_continues_with_attack_phase()
        {
            Sut.PlaceDraftArmies(_region, 3);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(ConqueringAchievement.NoTerritoryHasBeenConquered);
        }

        [Fact]
        public void Placing_more_draft_armies_than_left_throws()
        {
            Action act = () => Sut.PlaceDraftArmies(_region, 4);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}