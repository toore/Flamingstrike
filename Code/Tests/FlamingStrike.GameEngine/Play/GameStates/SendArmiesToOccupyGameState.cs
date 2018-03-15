using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FluentAssertions.Common;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Play.GameStates
{
    public class SendArmiesToOccupyGameStateTests
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;
        private readonly IReadOnlyList<ITerritory> _territories = new List<ITerritory>();

        public SendArmiesToOccupyGameStateTests()
        {
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _territoryOccupier = Substitute.For<ITerritoryOccupier>();

            _attackingRegion = Substitute.For<IRegion>();
            _occupiedRegion = Substitute.For<IRegion>();
        }

        private SendArmiesToOccupyPhase Sut => new SendArmiesToOccupyPhase(
            _gamePhaseConductor,
            null,
            _territories,
            null,
            null,
            _attackingRegion,
            _occupiedRegion,
            _territoryOccupier);

        [Fact]
        public void Sending_armies_to_occupy_continues_with_attack_state()
        {
            var expectedUpdatedTerritories = new List<ITerritory>();
            _territoryOccupier.SendInAdditionalArmiesToOccupy(
                _territories,
                _attackingRegion,
                _occupiedRegion,
                1).Returns(expectedUpdatedTerritories);

            Sut.SendAdditionalArmiesToOccupy(1);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(
                ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }
    }
}