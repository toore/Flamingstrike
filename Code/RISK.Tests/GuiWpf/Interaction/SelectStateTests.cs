﻿using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class SelectStateTests : InteractionStateTestsBase
    {
        public SelectStateTests()
        {
            _sut.CurrentState = new SelectState(_interactionStateFactory);
        }

        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.AssertCanClick(territory);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritoryId>());
        }

        [Fact]
        public void Click_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState().Returns(expected);
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.CurrentState.Should().Be(expected);
        }

        [Fact]
        public void When_entering_attack_state_the_selected_territory_is_updated()
        {
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.SelectedTerritoryId.Should().Be(territory);
        }

        private ITerritoryId CreateTerritoryOccupiedByCurrentPlayer()
        {
            var territory = Substitute.For<ITerritoryId>();
            _game.IsCurrentPlayerOccupyingTerritory(territory).Returns(true);
            return territory;
        }
    }
}