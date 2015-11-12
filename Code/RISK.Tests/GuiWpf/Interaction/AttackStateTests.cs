using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryId _selectedTerritoryId;

        public AttackStateTests()
        {
            _selectedTerritoryId = Substitute.For<ITerritoryId>();

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedTerritoryId = _selectedTerritoryId;
        }

        [Fact]
        public void Can_click_territory_that_can_be_attacked()
        {
            var territoryIdToAttack = Substitute.For<ITerritoryId>();
            _game.CanAttack(_selectedTerritoryId, territoryIdToAttack).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(territoryIdToAttack);
        }

        [Fact]
        public void OnClick_attacks()
        {
            var attackeeCandidate = Substitute.For<ITerritoryId>();
            _game.CanAttack(_selectedTerritoryId, attackeeCandidate).Returns(true);

            _sut.OnClick(attackeeCandidate);

            _game.Received().Attack(_selectedTerritoryId, attackeeCandidate);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(Substitute.For<ITerritoryId>());
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedTerritoryId);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState().Returns(selectState);

            _sut.OnClick(_selectedTerritoryId);

            _sut.CurrentState.Should().Be(selectState);
            _sut.SelectedTerritoryId.Should().BeNull();
        }

        [Fact]
        public void OnClick_resets_selected_territory_before_entering_select_state()
        {
            _sut.OnClick(_selectedTerritoryId);

            _sut.SelectedTerritoryId.Should().BeNull();
        }
    }
}