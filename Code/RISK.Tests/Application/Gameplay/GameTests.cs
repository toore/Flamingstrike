using System.Linq;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private Game _sut;
        private IOrderedEnumerable<IPlayer> _players;
        private IWorldMap _worldMap;
        private ICardFactory _cardFactory;
        private IBattleCalculator _battleCalculator;
        private IPlayer _currentPlayer;
        private IPlayer _nextPlayer;

        public GameTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            var players = new[] { _currentPlayer, _nextPlayer }.OrderBy(x => x.PlayerOrderIndex);

            _sut = new Game(_players, _worldMap, _cardFactory, _battleCalculator);
        }

        // can not attack territories if any not in map
        // can not attack territories with same occupant
        // can not attack if no occupant

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            _sut.EndTurn();

            _currentPlayer.DidNotReceiveWithAnyArgs().AddCard(null);
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            var card = Make.Card.Build();
            _cardFactory.Create().Returns(card);

            _sut.EndTurn();

            _currentPlayer.Received().AddCard(card);
        }
    }
}