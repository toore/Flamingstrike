using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritory _selectedTerritory;
        private readonly ITerritoryGeography _selectedTerritoryGeography;
        private readonly ITerritory _attackedTerritory;
        private readonly ITerritoryGeography _attackeeTerritoryGeography;

        public AttackStateTests()
        {
            _selectedTerritory = Substitute.For<ITerritory>();
            _selectedTerritoryGeography = _selectedTerritory.TerritoryGeography;
            _game.GetTerritory(_selectedTerritoryGeography).Returns(_selectedTerritory);

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedTerritoryGeography = _selectedTerritoryGeography;

            _attackedTerritory = Substitute.For<ITerritory>();
            _attackeeTerritoryGeography = Substitute.For<ITerritoryGeography>();
            _game.GetTerritory(_attackeeTerritoryGeography).Returns(_attackedTerritory);
        }

        [Fact]
        public void Can_click_territory_that_can_be_attacked()
        {
            _game.CanAttack(_selectedTerritory, _attackedTerritory).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(_attackeeTerritoryGeography);
        }

        [Fact]
        public void OnClick_attacks()
        {
            _game.CanAttack(_selectedTerritory, _attackedTerritory).Returns(true);

            _sut.OnClick(_attackeeTerritoryGeography);

            _game.Received().Attack(_selectedTerritory, _attackedTerritory);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(_attackeeTerritoryGeography);
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