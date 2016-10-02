using System;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace Tests.RISK.GameEngine.Play.GameStates
{
    public class DraftArmiesPhaseGameStateTests
    {
        private readonly IPlayer _currentPlayer;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private int _numberOfArmiesToDraft;

        public DraftArmiesPhaseGameStateTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _territoriesContext = Substitute.For<ITerritoriesContext>();
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _armyDrafter = Substitute.For<IArmyDrafter>();

            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            territory.Region.Returns(_region);
            territory.Player.Returns(_currentPlayer);
            anotherTerritory.Region.Returns(_anotherRegion);

            _territoriesContext.Territories.Returns(new[] { territory, anotherTerritory });

            _numberOfArmiesToDraft = 0;
        }

        private DraftArmiesPhaseGameState Sut => new DraftArmiesPhaseGameState(
            _currentPlayer,
            _territoriesContext,
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

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void After_placing_draft_armies_continues_to_draft_armies()
        {
            _numberOfArmiesToDraft = 2;
            Sut.PlaceDraftArmies(_region, 1);

            _gamePhaseConductor.Received().ContinueToDraftArmies(1);
        }

        [Fact]
        public void After_placing_all_draft_armies_continues_with_attack_phase()
        {
            _numberOfArmiesToDraft = 2;
            Sut.PlaceDraftArmies(_region, 2);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(TurnConqueringAchievement.NoTerritoryHasBeenConquered);
        }

        [Fact]
        public void Placing_more_draft_armies_than_left_throws()
        {
            Action act = () => Sut.PlaceDraftArmies(_region, 2);

            act.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}