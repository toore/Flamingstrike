using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameAdapterTests
    {
        private readonly GameAdapter _sut;
        private readonly IInteractionState _currentPlayerInteractionState;
        private readonly IInteractionState _nextPlayerInteractionState;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _nextPlayer;
        private readonly IStateController _currentStateController;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private Game _game;

        public GameAdapterTests()
        {
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            var stateControllerFactory = Substitute.For<IStateControllerFactory>();
            _cardFactory = Substitute.For<ICardFactory>();

            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            _currentStateController = Substitute.For<IStateController>();
            var nextStateController = Substitute.For<IStateController>();

            _worldMap = Substitute.For<IWorldMap>();
            var players = new[] { _currentPlayer, _nextPlayer }.OrderBy(x => x.PlayerOrderIndex);
            _game = new Game(players, _worldMap, _cardFactory, null);
            stateControllerFactory.Create(_currentPlayer, _game).Returns(_currentStateController);
            stateControllerFactory.Create(_nextPlayer, _game).Returns(nextStateController);
            _interactionStateFactory.CreateSelectState(_currentStateController, _currentPlayer).Returns(_currentPlayerInteractionState);
            _interactionStateFactory.CreateSelectState(nextStateController, _nextPlayer).Returns(_nextPlayerInteractionState);


            _sut = new GameAdapter(_interactionStateFactory, stateControllerFactory, _game);
        }

        [Fact]
        public void Has_world_map()
        {
            _sut.WorldMap.Should().Be(_worldMap);
        }

        //[Fact]
        //public void Gets_current_interaction_state()
        //{
        //    var actual = _sut.CurrentInteractionState;

        //    actual.Should().Be(_currentPlayerInteractionState);
        //}

        //[Fact]
        //public void Gets_current_interaction_state_when_state_has_changed()
        //{
        //    var newInteractionState = Substitute.For<IInteractionState>();
        //    _currentStateController.CurrentState = newInteractionState;

        //    _sut.CurrentInteractionState.Should().Be(newInteractionState);
        //}

        //[Fact]
        //public void Gets_turn_after_next_turn()
        //{
        //    _sut.EndTurn();

        //    var actual = _sut.CurrentInteractionState;

        //    actual.Should().Be(_nextPlayerInteractionState);
        //}

        [Fact]
        public void Ending_turn_moves_to_next_player()
        {
            _sut.EndTurn();

            _sut.Player.Should().Be(_nextPlayer);
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

            _currentStateController.CurrentState.Should().Be(fortify);
        }
    }
}