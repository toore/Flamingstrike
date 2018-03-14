using System;
using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Play.GameStates;
using FluentAssertions;
using FluentAssertions.Common;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Play.GameStates
{
    public class DraftArmiesPhaseGameStateTests
    {
        private readonly GameData _gameData;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private int _numberOfArmiesToDraft;

        public DraftArmiesPhaseGameStateTests()
        {
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _armyDrafter = Substitute.For<IArmyDrafter>();

            var currentPlayer = Substitute.For<IPlayer>();
            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            territory.Region.Returns(_region);
            territory.Player.Returns(currentPlayer);
            anotherTerritory.Region.Returns(_anotherRegion);

            _gameData = new GameDataBuilder()
                .CurrentPlayer(currentPlayer)
                .Territories(territory, anotherTerritory)
                .Build();

            _numberOfArmiesToDraft = 0;
        }

        private DraftArmiesPhaseGameState Sut => new DraftArmiesPhaseGameState(
            _gameData,
            _gamePhaseConductor,
            _armyDrafter,
            _numberOfArmiesToDraft);

        [Fact]
        public void Can_place_draft_armies_for_occupied_territory()
        {
            Sut.CanPlaceDraftArmies(_region).Should().BeTrue();
        }

        [Fact]
        public void Can_not_place_draft_armies_for_another_territory()
        {
            Sut.CanPlaceDraftArmies(_anotherRegion).Should().BeFalse();
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
            var expectedUpdatedTerritories = new List<ITerritory>();
            _armyDrafter.PlaceDraftArmies(_gameData.Territories, _region, 1).Returns(expectedUpdatedTerritories);

            _numberOfArmiesToDraft = 3;
            Sut.PlaceDraftArmies(_region, 1);

            _gamePhaseConductor.Received().ContinueToDraftArmies(2, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void After_placing_all_draft_armies_continues_with_attack_phase()
        {
            var expectedUpdatedTerritories = new List<ITerritory>();
            _armyDrafter.PlaceDraftArmies(_gameData.Territories, _region, 2).Returns(expectedUpdatedTerritories);

            _numberOfArmiesToDraft = 2;
            Sut.PlaceDraftArmies(_region, 2);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(ConqueringAchievement.NoTerritoryHasBeenConquered, Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }

        [Fact]
        public void Placing_more_draft_armies_than_left_throws()
        {
            Action act = () => Sut.PlaceDraftArmies(_region, 2);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}