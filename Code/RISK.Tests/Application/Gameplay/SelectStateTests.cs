using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class SelectStateTests
    {
        private readonly IInteractionState _sut;
        private readonly IPlayer _player;
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMap _worldMap;

        private readonly ILocation _locationOccupiedByPlayer;
        private readonly ITerritory _territoryOccupiedByPlayer;
        private readonly ILocation _locationOccupiedByOtherPlayer;

        public SelectStateTests()
        {
            _stateController = new StateController();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _player = Substitute.For<IPlayer>();
            _worldMap = Substitute.For<IWorldMap>();

            _sut = new SelectState(_stateController, _interactionStateFactory, Substitute.For<ICardFactory>(), _player, _worldMap);

            _locationOccupiedByPlayer = Make.Location.Build();
            _territoryOccupiedByPlayer = Make.Territory
                .Location(_locationOccupiedByPlayer)
                .Occupant(_player).Build();
            _worldMap.GetTerritory(_locationOccupiedByPlayer).Returns(_territoryOccupiedByPlayer);
            _locationOccupiedByOtherPlayer = Substitute.For<ILocation>();
        }

        [Fact]
        public void Can_click_location_occupied_by_player()
        {
            _sut.CanClick(_locationOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_click_location_not_occupied_by_player()
        {
            _sut.CanClick(_locationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Click_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState(_player, _worldMap, _territoryOccupiedByPlayer).Returns(expected);

            _sut.OnClick(_locationOccupiedByPlayer);

            _stateController.CurrentState.Should().Be(expected);
        }

        [Fact]
        public void Territory_is_not_selected_when_not_occupied_by_player()
        {
            var currentState = Substitute.For<IInteractionState>();
            _stateController.CurrentState = currentState;

            _sut.OnClick(_locationOccupiedByOtherPlayer);

            _stateController.CurrentState.Should().Be(currentState);
            _sut.SelectedTerritory.Should().BeNull();
        }
    }
}