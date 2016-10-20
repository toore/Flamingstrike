using System.Collections.Generic;
using FluentAssertions.Common;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GameStates;
using Tests.RISK.GameEngine.Builders;
using Xunit;

namespace Tests.RISK.GameEngine.Play.GameStates
{
    public class SendArmiesToOccupyGameStateTests
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;
        private readonly GameData _gameData;

        public SendArmiesToOccupyGameStateTests()
        {
            _gamePhaseConductor = Substitute.For<IGamePhaseConductor>();
            _territoryOccupier = Substitute.For<ITerritoryOccupier>();

            _attackingRegion = Substitute.For<IRegion>();
            _occupiedRegion = Substitute.For<IRegion>();

            _gameData = Make.GameData.Build();
        }

        private SendArmiesToOccupyGameState Sut => new SendArmiesToOccupyGameState(
            _gameData,
            _gamePhaseConductor,
            _territoryOccupier,
            _attackingRegion,
            _occupiedRegion);

        [Fact]
        public void Sending_armies_to_occupy_continues_with_attack_state()
        {
            var expectedUpdatedTerritories = new List<ITerritory>();
            _territoryOccupier.SendInAdditionalArmiesToOccupy(
                _gameData.Territories,
                _attackingRegion,
                _occupiedRegion,
                1).Returns(expectedUpdatedTerritories);

            Sut.SendAdditionalArmiesToOccupy(1);

            _gamePhaseConductor.Received().ContinueWithAttackPhase(
                TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory,
                Arg.Is<GameData>(x => x.Territories.IsSameOrEqualTo(expectedUpdatedTerritories)));
        }
    }
}