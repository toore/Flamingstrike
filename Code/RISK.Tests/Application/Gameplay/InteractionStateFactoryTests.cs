using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class InteractionStateFactoryTests
    {
        private readonly IBattleCalculator _battleCalculator;
        private readonly InteractionStateFactory _sut;
        private readonly IPlayer _player;

        public InteractionStateFactoryTests()
        {
            _battleCalculator = Substitute.For<IBattleCalculator>();

            _sut = new InteractionStateFactory(_battleCalculator);
            
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
    }
}