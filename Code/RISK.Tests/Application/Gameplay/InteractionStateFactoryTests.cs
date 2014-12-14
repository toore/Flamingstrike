using FluentAssertions;
using NSubstitute;
using RISK.Application.Entities;
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

            _sut = new InteractionStateFactory(battleCalculator);
            
            _player = Substitute.For<IPlayer>();
        }

        [Fact]
        public void Creates_select_state()
        {
            var actual = _sut.CreateSelectState(new StateController(), _player);

            actual.Should().BeOfType<SelectState>();
            actual.Player.Should().Be(_player);
            actual.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void Creates_attack_state()
        {
            var selectedTerritory = Substitute.For<ITerritory>();
            var actual = _sut.CreateAttackState(new StateController(), _player, selectedTerritory);

            actual.Should().BeOfType<AttackState>();
            actual.Player.Should().Be(_player);
            actual.SelectedTerritory.Should().Be(selectedTerritory);
        }

        [Fact]
        public void Creates_fortify_select_state()
        {
            var player = Substitute.For<IPlayer>();

            var interactionState = _sut.CreateFortifyState(new StateController(), player);

            interactionState.Should().BeOfType<FortifyState>();
            interactionState.Player.Should().Be(player);
        }
    }
}