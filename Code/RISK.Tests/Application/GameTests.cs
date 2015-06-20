using System;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Battling;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameTests
    {
        private Game _sut;
        private IWorldMap _worldMap;
        private ICardFactory _cardFactory;
        private IBattle _battle;
        private IPlayer _currentPlayer;
        private IPlayer _nextPlayer;

        public GameTests()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _nextPlayer = Substitute.For<IPlayer>();

            var players = new[] { _currentPlayer, _nextPlayer };

            _sut = new Game(null, _cardFactory, _battle);
        }

        // can not attack territories if any not in map
        // can not attack territories with same occupant
        // can not attack if no occupant

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = false;

            _sut.EndTurn();

            //_currentPlayerId.DidNotReceiveWithAnyArgs().AddCard(null);
            throw new NotImplementedException();
        }

        [Fact]
        public void Player_should_receive_card_when_turn_ends()
        {
            //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            var card = Make.Card.Build();
            _cardFactory.Create().Returns(card);

            _sut.EndTurn();

            //_currentPlayerId.Received().AddCard(card);
            throw new NotImplementedException();
        }
    }
}