using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private readonly Game _sut;
        private readonly IWorldMap _worldMap;
        private IInteractionStateFactory _interactionStateFactory;
        private readonly IInteractionState currentInteractionState;
        private readonly IInteractionState _nextInteractionState;
        private IPlayers _players;
        private IPlayer _currentPlayer;
        private IPlayer _nextPlayer;
        private StateController _currentStateController;
        private ICardFactory _cardFactory;

        public GameTests()
        {
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            var stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _players = Substitute.For<IPlayers>();
            _worldMap = Substitute.For<IWorldMap>();
            _cardFactory = Substitute.For<ICardFactory>();

            currentInteractionState = Substitute.For<IInteractionState>();
            _nextInteractionState = Substitute.For<IInteractionState>();
            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();
            _currentStateController = new StateController();
            var nextStateController = new StateController();
            stateControllerFactory.Create().Returns(_currentStateController, nextStateController);
            _interactionStateFactory.CreateSelectState(_currentStateController, _currentPlayer, _worldMap).Returns(currentInteractionState);
            _interactionStateFactory.CreateSelectState(nextStateController, _nextPlayer, _worldMap).Returns(_nextInteractionState);

            _players.GetAll().Returns(new[] { _currentPlayer, _nextPlayer });

            _sut = new Game(_interactionStateFactory, stateControllerFactory, _players, _worldMap, _cardFactory);
        }

        [Fact]
        public void Has_world_map()
        {
            _sut.WorldMap.Should().Be(_worldMap);
        }

        [Fact]
        public void Get_current_turn()
        {
            var actual = _sut.CurrentTurn;

            actual.Should().Be(currentInteractionState);
        }

        [Fact]
        public void Gets_turn_after_next_turn()
        {
            _sut.EndTurn();

            var actual = _sut.CurrentTurn;

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
    }
}