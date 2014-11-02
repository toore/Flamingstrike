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
        private IInteractionStateFactory _interactionStateFactory;
        private readonly IInteractionState currentInteractionState;
        private readonly IInteractionState _nextInteractionState;
        private IPlayers _players;
        private readonly IPlayer _currentPlayer;
        private IPlayer _nextPlayer;
        private readonly StateController _currentStateController;
        private readonly ICardFactory _cardFactory;
        private Territories _territories;

        public GameTests()
        {
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            var stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _players = Substitute.For<IPlayers>();
            _cardFactory = Substitute.For<ICardFactory>();

            currentInteractionState = Substitute.For<IInteractionState>();
            _nextInteractionState = Substitute.For<IInteractionState>();
            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();
            _currentStateController = new StateController();
            var nextStateController = new StateController();
            stateControllerFactory.Create().Returns(_currentStateController, nextStateController);
            _interactionStateFactory.CreateSelectState(_currentStateController, _currentPlayer).Returns(currentInteractionState);
            _interactionStateFactory.CreateSelectState(nextStateController, _nextPlayer).Returns(_nextInteractionState);

            _players.GetAll().Returns(new[] { _currentPlayer, _nextPlayer });

            _territories = new Territories();
            _sut = new Game(_interactionStateFactory, stateControllerFactory, _players, _territories, _cardFactory);
        }

        [Fact]
        public void Has_world_map()
        {
            _sut.Territories.Should().Be(_territories);
        }

        [Fact]
        public void Gets_current_interaction_state()
        {
            var actual = _sut.CurrentInteractionState;

            actual.Should().Be(currentInteractionState);
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
            _territories.GetAllPlayersOccupyingTerritories()
                .Returns(new[] { Substitute.For<IPlayer>() });

            _sut.IsGameOver().Should().BeTrue();
        }

        [Fact]
        public void Is_not_game_over_when_two_players_have_territories()
        {
            _territories.GetAllPlayersOccupyingTerritories()
                .Returns(new[]
                {
                    Substitute.For<IPlayer>(),
                    Substitute.For<IPlayer>()
                });

            _sut.IsGameOver().Should().BeFalse();
        }
    }
}