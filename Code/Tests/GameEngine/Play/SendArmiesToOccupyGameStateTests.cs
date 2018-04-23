using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using NSubstitute;
using Xunit;

namespace Tests.GameEngine.Play
{
    public class SendArmiesToOccupyGameStateTests
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly Region _attackingRegion;
        private readonly Region _occupiedRegion;
        private readonly IReadOnlyList<ITerritory> _territories;

        public SendArmiesToOccupyGameStateTests()
        {
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();

            _attackingRegion = Region.Brazil;
            _occupiedRegion = Region.NorthAfrica;

            _territories = new ITerritory[]
                {
                    new TerritoryBuilder().Region(_attackingRegion).Armies(2).Build(),
                    new TerritoryBuilder().Region(_occupiedRegion).Build(),
                };
        }

        private SendArmiesToOccupyPhase Sut => new SendArmiesToOccupyPhase(
            _gamePhaseConductor,
            null,
            _territories,
            null,
            null,
            _attackingRegion,
            _occupiedRegion);

        [Fact]
        public void Sending_armies_to_occupy_continues_with_attack_state()
        {
            Sut.SendAdditionalArmiesToOccupy(1);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(ConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory);
        }
    }
}