using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.Setup;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameTests
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly ITerritoryConverter _territoryConverter;
        private readonly GameFactory _gameFactory;

        public GameTests()
        {
            _gameRules = Substitute.For<IGameRules>();
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();
            _territoryConverter = Substitute.For<ITerritoryConverter>();

            _gameFactory = new GameFactory(_gameRules, _cardFactory, _battle, _territoryConverter);
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
        public void End_turn_passes_turn_to_next_player()
        {
            var initialPlayer = Substitute.For<IPlayer>();
            var nextPlayer = Substitute.For<IPlayer>();
            var gameSetup = Make.GameSetup
                .WithPlayer(initialPlayer)
                .WithPlayer(nextPlayer)
                .Build();
            
            var sut = Create(gameSetup);
            sut.EndTurn();

            sut.CurrentPlayer.Should().Be(nextPlayer);
        }

        [Fact]
        public void Initializes_gameboard_territories()
        {
            var gameSetup = Make.GameSetup.Build();
            var expected = new List<GameboardTerritory> { Make.GameboardTerritory.Build() };
            _territoryConverter.Convert(gameSetup.GameboardSetupTerritories).Returns(expected);

            var sut = Create(gameSetup);

            sut.GameboardTerritories.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void Is_game_over_when_all_territories_belongs_to_one_player()
        {
            var player = Substitute.For<IPlayer>();
            _territoryConverter.Convert(null).ReturnsForAnyArgs(new List<GameboardTerritory>
            {
                Make.GameboardTerritory.Player(player).Build(),
                Make.GameboardTerritory.Player(player).Build()
            });

            var sut = Create(Make.GameSetup.Build());

            sut.IsGameOver().Should().BeTrue();
        }

        [Fact]
        public void Is_not_game_over_when_more_than_one_player_occupies_territories()
        {
            _territoryConverter.Convert(null).ReturnsForAnyArgs(new List<GameboardTerritory>
            {
                Make.GameboardTerritory.Build(),
                Make.GameboardTerritory.Build()
            });

            var sut = Create(Make.GameSetup.Build());

            sut.IsGameOver().Should().BeFalse();
        }

        [Fact]
        public void Can_attack()
        {
            var attackingTerritory = Substitute.For<ITerritory>();
            var attackedTerritory = Substitute.For<ITerritory>();
            var gameboardTerritories = HasGameboardTerritories();
            _gameRules.GetAttackeeCandidates(attackingTerritory, gameboardTerritories)
                .Returns(new[] { attackedTerritory });

            var sut = Create(Make.GameSetup.Build());

            sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeTrue();
        }

        [Fact]
        public void Can_not_attack()
        {
            var attackingTerritory = Substitute.For<ITerritory>();
            var attackedTerritory = Substitute.For<ITerritory>();
            var gameboardTerritories = HasGameboardTerritories();
            _gameRules.GetAttackeeCandidates(attackingTerritory, gameboardTerritories)
                .Returns(new[] { Substitute.For<ITerritory>() });

            var sut = Create(Make.GameSetup.Build());

            sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeFalse();
        }

        private List<GameboardTerritory> HasGameboardTerritories()
        {
            var gameboardTerritories = new List<GameboardTerritory>();
            _territoryConverter.Convert(null).ReturnsForAnyArgs(gameboardTerritories);

            return gameboardTerritories;
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _gameFactory.Create(gamePlaySetup);
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