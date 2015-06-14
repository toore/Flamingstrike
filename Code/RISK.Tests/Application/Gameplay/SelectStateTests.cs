﻿using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class SelectStateTests
    {
        private readonly SelectState _sut;
        private readonly IPlayerId _playerId;
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        private readonly ITerritory _territoryOccupiedByPlayer;
        private readonly ITerritory _locationOccupiedByOtherPlayer;

        public SelectStateTests()
        {
            _stateController = new StateController(_interactionStateFactory, _playerId, null);
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _playerId = Substitute.For<IPlayerId>();

            _sut = new SelectState(_stateController, _interactionStateFactory, _playerId);

            _territoryOccupiedByPlayer = Make.Territory
                .Occupant(_playerId)
                .Build();
            _locationOccupiedByOtherPlayer = Substitute.For<ITerritory>();
        }

        [Fact]
        public void Can_click_location_occupied_by_player()
        {
            _sut.CanClick(_territoryOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Can_not_click_location_not_occupied_by_player()
        {
            _sut.AssertCanNotClick(_locationOccupiedByOtherPlayer);
        }

        [Fact]
        public void Click_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState(_stateController, _playerId, _territoryOccupiedByPlayer).Returns(expected);

            _sut.OnClick(_territoryOccupiedByPlayer);

            _stateController.CurrentState.Should().Be(expected);
        }
    }
}