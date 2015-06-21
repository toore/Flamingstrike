using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Battling;
using RISK.Application.Setup;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameTests
    {
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly GameFactory _gameFactory;

        public GameTests()
        {
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();

            _gameFactory = new GameFactory(_cardFactory, _battle);
        }

        [Fact]
        public void Initializes_with_first_player_taking_turn_first()
        {
            var firstPlayer = Substitute.For<IPlayer>();
            var players = new[] { firstPlayer, Substitute.For<IPlayer>(), Substitute.For<IPlayer>() };
            var gameSetup = new GameSetup(players, null);

            var sut = Create(gameSetup);

            sut.GameState.CurrentPlayer.Should().Be(firstPlayer);
        }

        [Fact]
        public void Initializes_with_territories()
        {
            var players = new[] { Substitute.For<IPlayer>() };
            //var territories = new List<IGameboardSetupTerritory>
            //{
            //    Substitute.For<IGameboardSetupTerritory>(),
            //    Substitute.For<IGameboardSetupTerritory>(),
            //    Substitute.For<IGameboardSetupTerritory>()
            //};
            var gameSetup = new GameSetup(players, null);

            var sut = Create(gameSetup);

            //sut.GameState.Territories
        }

        private IGame Create(IGameSetup gameSetup)
        {
            return _gameFactory.Create(gameSetup);
        }

        // can not attack territories if any not in map
        // can not attack territories with same occupant
        // can not attack if no occupant

        //[Fact]
        //public void Player_should_not_receive_card_when_turn_ends()
        //{
        //    //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = false;

        //    _sut.EndTurn();

        //    //_currentPlayerId.DidNotReceiveWithAnyArgs().AddCard(null);
        //    throw new NotImplementedException();
        //}

        //[Fact]
        //public void Player_should_receive_card_when_turn_ends()
        //{
        //    //_currentStateController.PlayerShouldReceiveCardWhenTurnEnds = true;
        //    var card = Make.Card.Build();
        //    _cardFactory.Create().Returns(card);

        //    _sut.EndTurn();

        //    //_currentPlayerId.Received().AddCard(card);
        //    throw new NotImplementedException();
        //}
    }
}