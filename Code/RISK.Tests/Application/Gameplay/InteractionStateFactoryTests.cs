using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class InteractionStateFactoryTests
    {
        private readonly InteractionStateFactory _sut;
        private readonly IPlayer _player;

        public InteractionStateFactoryTests()
        {
            var battleCalculator = Substitute.For<IBattleCalculator>();

            _sut = new InteractionStateFactory();

            _player = Substitute.For<IPlayer>();
        }

        [Fact]
        public void Creates_select_state()
        {
            var actual = _sut.CreateSelectState(null, _player);

            actual.Should().BeOfType<SelectState>();
            actual.Player.Should().Be(_player);
            actual.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Creates_attack_state()
        {
            var selectedTerritory = Substitute.For<ITerritory>();
            var actual = _sut.CreateAttackState(Substitute.For<IStateController>(), _player, selectedTerritory);

            actual.Should().BeOfType<AttackState>();
            actual.Player.Should().Be(_player);
            actual.SelectedTerritory.Should().Be(selectedTerritory);
        }

        [Fact]
        public void Creates_fortify_state()
        {
            var player = Substitute.For<IPlayer>();

            var interactionState = _sut.CreateFortifyState(Substitute.For<IStateController>(), player);

            interactionState.Should().BeOfType<FortifySelectState>();
            interactionState.Player.Should().Be(player);
            interactionState.SelectedTerritory.Should().BeNull("user will select which territory to fortify from");
        }

        [Fact]
        public void Creates_fortify_with_selected_territory_state()
        {
            var player = Substitute.For<IPlayer>();
            var selectedTerritory = Substitute.For<ITerritory>();

            var interactionState = _sut.CreateFortifyState(Substitute.For<IStateController>(), player, selectedTerritory);

            interactionState.Should().BeOfType<FortifyMoveState>();
            interactionState.Player.Should().Be(player);
            interactionState.SelectedTerritory.Should().Be(selectedTerritory);
        }
    }
}