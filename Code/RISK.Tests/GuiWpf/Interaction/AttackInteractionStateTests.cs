using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class AttackInteractionStateTests
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;
        private readonly IRegion _selectedRegion;
        private readonly AttackInteractionState _sut;
        private readonly IRegion _attackedRegion;

        public AttackInteractionStateTests()
        {
            _interactionStateFsm = Substitute.For<IInteractionStateFsm>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _game = Substitute.For<IGame>();
            _selectedRegion = Substitute.For<IRegion>();

            _sut = new AttackInteractionState(_interactionStateFsm, _interactionStateFactory, _game, _selectedRegion);

            _attackedRegion = Substitute.For<IRegion>();
        }

        [Fact]
        public void Selected_region_is_defined()
        {
            _sut.SelectedRegion.Should().Be(_selectedRegion);
        }

        [Fact]
        public void Can_click_territory_that_can_be_attacked()
        {
            _game.CanAttack(_selectedRegion, _attackedRegion).Returns(true);

            _sut.AssertCanClickAndOnClickCanBeInvoked(_attackedRegion);
        }

        [Fact]
        public void OnClick_attacks()
        {
            _game.CanAttack(_selectedRegion, _attackedRegion).Returns(true);

            _sut.OnClick(_attackedRegion);

            _game.Received().Attack(_selectedRegion, _attackedRegion);
        }

        [Fact]
        public void Can_not_click_on_remote_region()
        {
            _sut.AssertCanNotClickAndOnClickThrowsInvalidOperationException(_attackedRegion);
        }

        [Fact]
        public void Can_click_on_selected_region()
        {
            _sut.AssertCanClickAndOnClickCanBeInvoked(_selectedRegion);
        }

        [Fact]
        public void OnClick_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectInteractionState(_game).Returns(selectState);

            _sut.OnClick(_selectedRegion);

            _interactionStateFsm.Received().Set(selectState);
        }
    }
}