using System;
using System.Linq;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.GamePlay.Battling;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private Game _sut;
        private IWorldMap _worldMap;
        private ICardFactory _cardFactory;
        private IBattle _battle;
        private IPlayerId _currentPlayerId;
        private IPlayerId _nextPlayerId;

        public GameTests()
        {
            _currentPlayerId = Substitute.For<IPlayerId>();
            _nextPlayerId = Substitute.For<IPlayerId>();

            var players = new[] { _currentPlayerId, _nextPlayerId };

            _sut = new Game(players, _worldMap, _cardFactory, _battle);
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