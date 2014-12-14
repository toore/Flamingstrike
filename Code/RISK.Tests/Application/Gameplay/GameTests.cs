﻿using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private readonly Game _sut;
        private readonly IInteractionState _currentPlayerInteractionState;
        private readonly IInteractionState _nextPlayerInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly StateController _currentStateController;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public GameTests()
        {
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            var stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _cardFactory = Substitute.For<ICardFactory>();

            _currentPlayerInteractionState = Substitute.For<IInteractionState>();
            _nextPlayerInteractionState = Substitute.For<IInteractionState>();
            _currentPlayer = Substitute.For<IPlayer>();
            var nextPlayer = Substitute.For<IPlayer>();
            _currentStateController = new StateController();
            var nextStateController = new StateController();
            stateControllerFactory.Create().Returns(_currentStateController, nextStateController);
            _interactionStateFactory.CreateSelectState(_currentStateController, _currentPlayer).Returns(_currentPlayerInteractionState);
            _interactionStateFactory.CreateSelectState(nextStateController, nextPlayer).Returns(_nextPlayerInteractionState);

            _worldMap = Substitute.For<IWorldMap>();
            _sut = new Game(_interactionStateFactory, stateControllerFactory, new[] { _currentPlayer, nextPlayer }, _worldMap, _cardFactory);
        }

        [Fact]
        public void Has_world_map()
        {
            _sut.WorldMap.Should().Be(_worldMap);
        }

        [Fact]
        public void Gets_current_interaction_state()
        {
            var actual = _sut.CurrentInteractionState;

            actual.Should().Be(_currentPlayerInteractionState);
        }

        [Fact]
        public void Gets_current_interaction_state_when_state_has_changed()
        {
            var newInteractionState = Substitute.For<IInteractionState>();
            _currentStateController.CurrentState = newInteractionState;

            _sut.CurrentInteractionState.Should().Be(newInteractionState);
        }

        [Fact]
        public void Gets_turn_after_next_turn()
        {
            _sut.EndTurn();

            var actual = _sut.CurrentInteractionState;

            actual.Should().Be(_nextPlayerInteractionState);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            _currentStateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            var card = Make.Card.Build();
            _cardFactory.Create().Returns(card);

            _sut.EndTurn();

            _currentPlayer.Received().AddCard(card);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            _currentStateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            _sut.EndTurn();

            _currentPlayer.DidNotReceiveWithAnyArgs().AddCard(null);
        }

        [Fact]
        public void Is_game_over_when_only_one_player_has_territories()
        {
            _worldMap.GetAllPlayersOccupyingTerritories()
                .Returns(new[] { Substitute.For<IPlayer>() });

            _sut.IsGameOver().Should().BeTrue();
        }

        [Fact]
        public void Is_not_game_over_when_two_players_have_territories()
        {
            _worldMap.GetAllPlayersOccupyingTerritories()
                .Returns(new[]
                {
                    Substitute.For<IPlayer>(),
                    Substitute.For<IPlayer>()
                });

            _sut.IsGameOver().Should().BeFalse();
        }

        [Fact]
        public void Game_enters_fortify_state()
        {
            var fortify = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateFortifyState(_currentStateController, _currentPlayer).Returns(fortify);
            
            _sut.Fortify();

            _sut.CurrentInteractionState.Should().Be(fortify);
        }
    }
}