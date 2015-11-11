using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class FortifyMoveStateTests : InteractionStateTestsBase
    {
        public FortifyMoveStateTests()
        {
            _sut.CurrentState = new FortifyMoveState(_interactionStateFactory);
        }

        [Fact]
        public void Can_click_fortification_candidate()
        {
            var fortificationCandidate = Substitute.For<ITerritoryId>();
            var selectedTerritoryId = Substitute.For<ITerritoryId>();
            _game.CanFortify(selectedTerritoryId, fortificationCandidate).Returns(true);
            _sut.SelectedTerritoryId = selectedTerritoryId;

            _sut.AssertCanClickAndCanBeInvoked(fortificationCandidate);
        }

        [Fact]
        public void Click_on_fortification_candidate_fortifies()
        {
            // inte egen
            // inte annans
            throw new NotImplementedException();
        }

        [Fact]
        public void Click_enters_end_turn_state()
        {
            var endTurnState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateEndTurnState().Returns(endTurnState);
            ITerritoryId adjacentAndOccupiedTerritory = null;// CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(adjacentAndOccupiedTerritory);

            _sut.CurrentState.Should().Be(endTurnState);
        }
    }
}