using System.Collections.Generic;
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
        private readonly IGameboardRules _gameboardRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly ITerritoryConverter _territoryConverter;
        private readonly GameFactory _gameFactory;

        public GameTests()
        {
            _gameboardRules = Substitute.For<IGameboardRules>();
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();
            _territoryConverter = Substitute.For<ITerritoryConverter>();

            _gameFactory = new GameFactory(_gameboardRules, _cardFactory, _battle, _territoryConverter);
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

            sut.CurrentPlayer.Should().Be(firstPlayer);
        }

        [Fact]
        public void Initializes_territories()
        {
            var gameSetup = Make.GameSetup.Build();
            var expected = new List<GameboardTerritory> { null };
            _territoryConverter.Convert(gameSetup.GameboardTerritories).Returns(expected);

            var sut = Create(gameSetup);
            var actual = sut.Territories;

            actual.Should().BeSameAs(expected);
        }

        [Fact]
        public void Get_attackee_candidates()
        {
            var attackingTerritory = Substitute.For<ITerritory>();
            IEnumerable<ITerritory> attackeeCandidates = new[]
            {
                Substitute.For<ITerritory>(),
                Substitute.For<ITerritory>(),
                Substitute.For<ITerritory>()
            };
            var gameboardTerritories = new List<GameboardTerritory>();
            _territoryConverter.Convert(null)
                .ReturnsForAnyArgs(gameboardTerritories);
            _gameboardRules.GetAttackeeCandidates(gameboardTerritories, attackingTerritory)
                .Returns(attackeeCandidates);

            var sut = Create(Make.GameSetup.Build());
            var actual = sut.GetAttackeeCandidates(attackingTerritory);

            actual.Should().BeEquivalentTo(attackeeCandidates);
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