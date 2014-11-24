using FluentAssertions;
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
        private readonly IInteractionState _currentInteractionState;
        private readonly IInteractionState _nextInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly StateController _currentStateController;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;

        public GameTests()
        {
            var interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            var stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _cardFactory = Substitute.For<ICardFactory>();

            _currentInteractionState = Substitute.For<IInteractionState>();
            _nextInteractionState = Substitute.For<IInteractionState>();
            _currentPlayer = Substitute.For<IPlayer>();
            var nextPlayer = Substitute.For<IPlayer>();
            _currentStateController = new StateController();
            var nextStateController = new StateController();
            stateControllerFactory.Create().Returns(_currentStateController, nextStateController);
            interactionStateFactory.CreateSelectState(_currentStateController, _currentPlayer).Returns(_currentInteractionState);
            interactionStateFactory.CreateSelectState(nextStateController, nextPlayer).Returns(_nextInteractionState);

            _worldMap = Substitute.For<IWorldMap>();
            _sut = new Game(interactionStateFactory, stateControllerFactory, new[] { _currentPlayer, nextPlayer }, _worldMap, _cardFactory);
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

            actual.Should().Be(_currentInteractionState);
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

            actual.Should().Be(_nextInteractionState);
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
    }
}