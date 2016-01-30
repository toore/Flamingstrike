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
        private readonly IRegion _selectedRegion;
        private readonly ITerritory _attackedTerritory;
        private readonly IRegion _attackeeRegion;

        public AttackStateTests()
        {
            _selectedTerritory = Substitute.For<ITerritory>();
            _selectedRegion = _selectedTerritory.Region;
            _game.GetTerritory(_selectedRegion).Returns(_selectedTerritory);

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedRegion = _selectedRegion;

            _attackedTerritory = Substitute.For<ITerritory>();
            _attackeeRegion = Substitute.For<IRegion>();
            _game.GetTerritory(_attackeeRegion).Returns(_attackedTerritory);
        }

        [Fact]
        public void Can_click_territory_that_can_be_attacked()
        {
            _game.CanAttack(_selectedTerritory, _attackedTerritory).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(_attackeeRegion);
        }

        [Fact]
        public void OnClick_attacks()
        {
            _game.CanAttack(_selectedTerritory, _attackedTerritory).Returns(true);

            _sut.OnClick(_attackeeRegion);

            _game.Received().Attack(_selectedTerritory, _attackedTerritory);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            _sut.AssertCanNotClickAndOnClickThrowsWhenInvoked(_attackeeRegion);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedRegion);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState().Returns(selectState);

            _sut.OnClick(_selectedRegion);

            _sut.CurrentState.Should().Be(selectState);
            _sut.SelectedRegion.Should().BeNull();
        }

        [Fact]
        public void OnClick_resets_selected_territory_before_entering_select_state()
        {
            _sut.OnClick(_selectedRegion);

            _sut.SelectedRegion.Should().BeNull();
        }
    }
}