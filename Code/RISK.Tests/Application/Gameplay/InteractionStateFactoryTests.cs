using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.GamePlay.Battling;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class InteractionStateFactoryTests
    {
        private readonly InteractionStateFactory _sut;
        private readonly IPlayerId _playerId;

        public InteractionStateFactoryTests()
        {
            var battleCalculator = Substitute.For<IBattle>();

            _sut = new InteractionStateFactory();

            _playerId = Substitute.For<IPlayerId>();
        }

        [Fact]
        public void Creates_select_state()
        {
            var actual = _sut.CreateSelectState(null, _playerId);

            actual.Should().BeOfType<SelectState>();
            actual.PlayerId.Should().Be(_playerId);
            actual.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Creates_attack_state()
        {
            var selectedTerritory = Substitute.For<ITerritory>();
            var actual = _sut.CreateAttackState(Substitute.For<IStateController>(), _playerId, selectedTerritory);

            actual.Should().BeOfType<AttackState>();
            actual.PlayerId.Should().Be(_playerId);
            actual.SelectedTerritory.Should().Be(selectedTerritory);
        }

        [Fact]
        public void Creates_fortify_state()
        {
            var player = Substitute.For<IPlayerId>();

            var interactionState = _sut.CreateFortifyState(Substitute.For<IStateController>(), player);

            interactionState.Should().BeOfType<FortifySelectState>();
            interactionState.PlayerId.Should().Be(player);
            interactionState.SelectedTerritory.Should().BeNull("user will select which territory to fortify from");
        }

        [Fact]
        public void Creates_fortify_with_selected_territory_state()
        {
            var player = Substitute.For<IPlayerId>();
            var selectedTerritory = Substitute.For<ITerritory>();

            var interactionState = _sut.CreateFortifyState(Substitute.For<IStateController>(), player, selectedTerritory);

            interactionState.Should().BeOfType<FortifyMoveState>();
            interactionState.PlayerId.Should().Be(player);
            interactionState.SelectedTerritory.Should().Be(selectedTerritory);
        }
    }
}