using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnAttackStateTransitionsTests
    {
        [Fact]
        public void Selecting_selected_location_enters_select_state()
        {
            var player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();
            var turnStateFactory = Substitute.For<ITurnStateFactory>();
            var selectedLocation = Substitute.For<ILocation>();
            var selectedTerritory = worldMap.HasTerritory(selectedLocation, player);
            var expected = Substitute.For<ITurnState>();
            var stateController = new StateController();
            turnStateFactory.CreateSelectState(player, worldMap).Returns(expected);

            var sut = new TurnAttackState(stateController, turnStateFactory, null, null, player, worldMap, selectedTerritory);
            sut.Select(selectedLocation);

            stateController.CurrentState.Should().Be(expected);
        }

        [Fact]
        public void After_successfull_attack_enters_attack_state()
        {
            var player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();
            var turnStateFactory = Substitute.For<ITurnStateFactory>();
            var battleCalculator = Substitute.For<IBattleCalculator>();
            var selectedLocation = Substitute.For<ILocation>();
            var otherLocation = Substitute.For<ILocation>();
            var expected = Substitute.For<ITurnState>();
            var stateController = new StateController();
            selectedLocation.IsConnectedTo(otherLocation);
            var selectedTerritory = worldMap.HasTerritory(selectedLocation, player);
            selectedTerritory.Armies = 2;
            var adjacentTerritory = worldMap.HasTerritory(otherLocation, player);
            adjacentTerritory.Occupant = Substitute.For<IPlayer>();
            adjacentTerritory.Armies = 2;
            battleCalculator.AttackerAlwaysWins(selectedTerritory, adjacentTerritory);
            turnStateFactory.CreateAttackState(player, worldMap, adjacentTerritory).Returns(expected);

            var sut = new TurnAttackState(stateController, turnStateFactory, battleCalculator, null, player, worldMap, selectedTerritory);
            sut.Attack(otherLocation);

            stateController.CurrentState.Should().Be(expected);
        }

        //[Fact]
        //public void Start_fortifying_enters_fortifying_state()
        //{
        //    var player = Substitute.For<IPlayer>();
        //    var worldMap = Substitute.For<IWorldMap>();
        //    var turnStateFactory = Substitute.For<ITurnStateFactory>();
        //    var battleCalculator = Substitute.For<IBattleCalculator>();
        //    var selectedLocation = Substitute.For<ILocation>();
        //    var otherLocation = Substitute.For<ILocation>();
        //    var selectedTerritory = worldMap.HasTerritory(selectedLocation, player);
        //    var otherTerritory = Substitute.For<ITerritory>();
        //    otherTerritory.Location.Returns(otherLocation);
        //    var expected = Substitute.For<ITurnState>();
        //    var stateController = new StateController();
        //    selectedLocation.IsConnectedTo(otherLocation);
        //    selectedTerritory.Armies = 2;
        //    battleCalculator.AttackerAlwaysWins(selectedTerritory, otherTerritory);
        //    turnStateFactory.CreateFortifyingState(player, worldMap, selectedTerritory).Returns(expected);

        //    var sut = new TurnAttackState(stateController, turnStateFactory, battleCalculator, null, player, worldMap, selectedTerritory);
        //    sut.Attack(otherLocation);

        //    stateController.CurrentState.Should().Be(expected);
        //}
    }
}