using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class InteractionStateFactoryTests
    {
        private readonly InteractionStateFactory _sut;

        public InteractionStateFactoryTests()
        {
            _sut = new InteractionStateFactory();
        }

        [Fact]
        public void Creates_select_state()
        {
            _sut.CreateSelectState().Should().BeOfType<SelectState>();
        }

        [Fact]
        public void Creates_attack_state()
        {
            _sut.CreateAttackState().Should().BeOfType<AttackState>();
        }

        [Fact]
        public void Creates_fortify_select_state()
        {
            _sut.CreateFortifySelectState().Should().BeOfType<FortifySelectState>();
        }

        [Fact]
        public void Creates_fortify_move_state()
        {
            _sut.CreateFortifyMoveState().Should().BeOfType<FortifyMoveState>();
        }

        [Fact]
        public void Creates_end_turn_state()
        {
            _sut.CreateEndTurnState().Should().BeOfType<EndTurnState>();
        }
    }
}