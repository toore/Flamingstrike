using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Battling;
using RISK.Application.Setup;
using RISK.Application.World;
using RISK.Tests.Builders;
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
            var gameSetup = Make.GameSetup
                .WithPlayer(firstPlayer)
                .WithPlayer(Substitute.For<IPlayer>())
                .WithPlayer(Substitute.For<IPlayer>())
                .Build();

            var sut = Create(gameSetup);

            sut.GameState.CurrentPlayer.Should().Be(firstPlayer);
        }

        [Fact]
        public void Initializes_territories()
        {
            var territory1 = Substitute.For<ITerritory>();
            var territory2 = Substitute.For<ITerritory>();
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();
            var gameSetup = Make.GameSetup
                .WithTerritory(new GameboardSetupTerritory(territory1, player1, 1))
                .WithTerritory(new GameboardSetupTerritory(territory2, player2, 2))
                .Build();

            var sut = Create(gameSetup);

            sut.GameState.Territories.ShouldAllBeEquivalentToInRisk(new[]
            {
                new GameboardTerritory(territory1, player1, 1),
                new GameboardTerritory(territory2, player2, 2),
            });
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