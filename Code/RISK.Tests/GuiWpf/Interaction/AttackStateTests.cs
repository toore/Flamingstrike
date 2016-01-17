using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryGeography _selectedTerritoryGeography;
        private readonly Territory _selectedTerritory;

        public AttackStateTests()
        {
            _selectedTerritory = AddTerritory();
            _selectedTerritoryGeography = _selectedTerritory.TerritoryGeography;

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedTerritoryGeography = _selectedTerritoryGeography;
        }

        [Fact]
        public void Can_click_territory_that_can_be_attacked()
        {
            var attackeeTerritory = AddTerritory();
            _game.CanAttack(_selectedTerritory, attackeeTerritory).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(attackeeTerritory.TerritoryGeography);
        }

        [Fact]
        public void OnClick_attacks()
        {
            var attackeeTerritory = AddTerritory();
            _game.CanAttack(_selectedTerritory, attackeeTerritory).Returns(true);

            _sut.OnClick(attackeeTerritory.TerritoryGeography);

            _game.Received().Attack(_selectedTerritory, attackeeTerritory);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            var attackeeTerritory = AddTerritory();
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(attackeeTerritory.TerritoryGeography);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedTerritoryGeography);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState().Returns(selectState);

            _sut.OnClick(_selectedTerritoryGeography);

            _sut.CurrentState.Should().Be(selectState);
            _sut.SelectedTerritoryGeography.Should().BeNull();
        }

        [Fact]
        public void OnClick_resets_selected_territory_before_entering_select_state()
        {
            _sut.OnClick(_selectedTerritoryGeography);

            _sut.SelectedTerritoryGeography.Should().BeNull();
        }
    }
}