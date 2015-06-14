using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class FortifySelectStateTests
    {
        private readonly FortifySelectState _sut;
        private readonly IPlayerId _playerId;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly StateController _stateController;
        private readonly Territory _territoryOccupiedByPlayer;

        public FortifySelectStateTests()
        {
            _stateController = new StateController(_interactionStateFactory, _playerId, null);
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _playerId = Substitute.For<IPlayerId>();

            _territoryOccupiedByPlayer = Make.Territory
                .Occupant(_playerId)
                .Build();

            _sut = new FortifySelectState(_stateController, _interactionStateFactory, _playerId);
        }

        [Fact]
        public void Can_click_occupied_territory()
        {
            _sut.CanClick(_territoryOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_click_not_occupied_territory()
        {
            var territory = Make.Territory.Build();

            _sut.AssertCanNotClick(territory);
        }

        [Fact]
        public void Clicking_occupied_territory_enters_fortify_state()
        {
            var fortifyState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifyState(_stateController, _playerId, _territoryOccupiedByPlayer).Returns(fortifyState);

            _sut.OnClick(_territoryOccupiedByPlayer);

            _stateController.CurrentState.Should().Be(fortifyState);
        }
    }

    public class FortifyMoveTests
    {
        private readonly FortifyMoveState _sut;
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IPlayerId _playerId;
        private readonly ITerritory _selectedTerritory;
        private readonly Territory _remoteTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByOtherPlayer;
        private readonly Territory _borderingTerritoryOccupiedByPlayer;
        private readonly Territory _remoteTerritoryOccupiedByPlayer;

        public FortifyMoveTests()
        {
            _stateController = new StateController(_interactionStateFactory, _playerId, null);
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _playerId = Substitute.For<IPlayerId>();

            _borderingTerritoryOccupiedByPlayer = Make.Territory
                .Occupant(_playerId)
                .Build();

            _remoteTerritoryOccupiedByPlayer = Make.Territory
                .Occupant(_playerId)
                .Build();

            _selectedTerritory = Make.Territory
                .WithBorder(_borderingTerritoryOccupiedByOtherPlayer)
                .WithBorder(_borderingTerritoryOccupiedByPlayer)
                .Occupant(_playerId)
                .Build();

            _sut = new FortifyMoveState(_playerId, _selectedTerritory);
        }

        [Fact]
        public void Can_not_fortify()
        {
            _sut.CanClick(_selectedTerritory).Should().BeFalse();
            _sut.CanClick(_borderingTerritoryOccupiedByOtherPlayer).Should().BeFalse();
            _sut.CanClick(_remoteTerritoryOccupiedByOtherPlayer).Should().BeFalse();
            _sut.CanClick(_remoteTerritoryOccupiedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_fortify_to_bordering_territory_occupied_by_player()
        {
            _sut.CanClick(_borderingTerritoryOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Fortifies_three_army_to_bordering_territory()
        {
            //_selectedTerritory.Armies = 4;
            //_borderingTerritoryOccupiedByPlayer.Armies = 10;

            //_sut.OnClick(_borderingTerritoryOccupiedByPlayer); //3 armies how?

            //_borderingTerritoryOccupiedByPlayer.Armies.Should().Be(13);
            //_selectedTerritory.Armies.Should().Be(1);
            throw new NotImplementedException();
        }

        [Fact]
        public void Enters_turn_end_state()
        {
            _sut.OnClick(_borderingTerritoryOccupiedByPlayer);

            _sut.OnClick(_borderingTerritoryOccupiedByPlayer);

            _stateController.CurrentState.Should().BeOfType<TurnEndState>();
        }
    }
}