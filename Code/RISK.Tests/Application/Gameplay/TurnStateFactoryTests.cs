using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnStateFactoryTests
    {
        private readonly StateController _stateController;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;
        private InteractionStateFactory _sut;
        private IPlayer _player;
        private IWorldMap _worldMap;

        public TurnStateFactoryTests()
        {
            _stateController = new StateController();
            _battleCalculator = Substitute.For<IBattleCalculator>();
            _cardFactory = Substitute.For<ICardFactory>();

            _sut = new InteractionStateFactory(_stateController, _battleCalculator, _cardFactory);

            _player = Substitute.For<IPlayer>();
            _worldMap = Substitute.For<IWorldMap>();
        }

        [Fact]
        public void Creates_select_state()
        {
            var actual = _sut.CreateSelectState(_player, _worldMap);

            actual.Should().BeOfType<SelectState>();
            actual.Player.Should().Be(_player);
            actual.IsTerritorySelected.Should().BeFalse();
        }

        [Fact]
        public void Creates_attack_state()
        {
            var selectedTerritory = Substitute.For<ITerritory>();
            var actual = _sut.CreateAttackState(_player, _worldMap, selectedTerritory);

            actual.Should().BeOfType<AttackState>();
            actual.Player.Should().Be(_player);
            actual.IsTerritorySelected.Should().BeTrue();
            actual.SelectedTerritory.Should().Be(selectedTerritory);
        }
    }
}