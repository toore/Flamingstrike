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
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.Application
{
    public abstract class GameTestsBase
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly ITerritoryFactory _territoryFactory;
        private readonly GameFactory _gameFactory;
        private readonly IPlayerFactory _playerFactory;

        protected GameTestsBase()
        {
            _gameRules = Substitute.For<IGameRules>();
            _cardFactory = Substitute.For<ICardFactory>();
            _battle = Substitute.For<IBattle>();
            _territoryFactory = Substitute.For<ITerritoryFactory>();
            _playerFactory = Substitute.For<IPlayerFactory>();

            _gameFactory = new GameFactory(_gameRules, _cardFactory, _battle, _territoryFactory, _playerFactory);
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _gameFactory.Create(gamePlaySetup);
        }

        private void HasPlayers()
        {
            var players = new List<IPlayer>(new[] { Substitute.For<IPlayer>() });
            _playerFactory.Create(null).ReturnsForAnyArgs(players);
        }

        private List<Territory> HasTerritories()
        {
            var territories = new List<Territory>();
            _territoryFactory.Create(null).ReturnsForAnyArgs(territories);

            return territories;
        }

        public class GameInitializationTests : GameTestsBase
        {
            [Fact]
            public void Initializes_with_first_player_taking_turn_first()
            {
                var gameSetup = Make.GameSetup.Build();
                var firstPlayer = Substitute.For<IPlayer>();
                _playerFactory.Create(gameSetup.Players).Returns(new List<IPlayer>
                {
                    firstPlayer,
                    Substitute.For<IPlayer>(),
                    Substitute.For<IPlayer>()
                });

                var sut = Create(gameSetup);

                sut.CurrentPlayer.Should().Be(firstPlayer);
            }

            [Fact]
            public void Initializes_territories()
            {
                var gameSetup = Make.GameSetup.Build();
                var expected = new List<Territory> { Make.Territory.Build() };
                _territoryFactory.Create(gameSetup.Territories).Returns(expected);
                HasPlayers();

                var sut = Create(gameSetup);

                sut.Territories.Should().BeEquivalentTo(expected);
            }
        }

        public class GameAttackTests : GameTestsBase
        {
            // can not attack territories if any not in map
            // can not attack territories with same occupant

            // can not attack with other player

            //Attack of bordering territory: occupied by player, occupied by other player
            //Attack of remote territory: occupied by player, occupied by other player

            [Fact]
            public void Can_attack()
            {
                var attackingTerritory = Substitute.For<ITerritoryId>();
                var attackedTerritory = Substitute.For<ITerritoryId>();
                var territories = HasTerritories();
                HasPlayers();
                _gameRules.GetAttackeeCandidates(attackingTerritory, territories)
                    .Returns(new[] { attackedTerritory });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeTrue();
            }

            [Fact]
            public void Can_not_attack()
            {
                var attackingTerritory = Substitute.For<ITerritoryId>();
                var attackedTerritory = Substitute.For<ITerritoryId>();
                var territories = HasTerritories();
                HasPlayers();
                _gameRules.GetAttackeeCandidates(attackingTerritory, territories)
                    .Returns(new[] { Substitute.For<ITerritoryId>() });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeFalse();
            }

            [Fact]
            public void Can_not_attack_with_another_players_territory()
            {
                var attackingTerritory = Substitute.For<ITerritoryId>();
                var attackedTerritory = Substitute.For<ITerritoryId>();
                var territories = HasTerritories();
                HasPlayers();
                _gameRules.GetAttackeeCandidates(attackingTerritory, territories)
                    .Returns(new[] { attackedTerritory });

                var sut = Create(Make.GameSetup.Build());

                sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeFalse();
            }
        }

        public class GameGameOverTests : GameTestsBase
        {
            [Fact]
            public void Is_game_over_when_all_territories_belongs_to_one_player()
            {
                var player = Substitute.For<IPlayerId>();
                _territoryFactory.Create(null).ReturnsForAnyArgs(new List<Territory>
                {
                    Make.Territory.Player(player).Build(),
                    Make.Territory.Player(player).Build()
                });
                HasPlayers();

                var sut = Create(Make.GameSetup.Build());

                sut.IsGameOver().Should().BeTrue();
            }

            [Fact]
            public void Is_not_game_over_when_more_than_one_player_occupies_territories()
            {
                _territoryFactory.Create(null).ReturnsForAnyArgs(new List<Territory>
                {
                    Make.Territory.Build(),
                    Make.Territory.Build()
                });
                HasPlayers();

                var sut = Create(Make.GameSetup.Build());

                sut.IsGameOver().Should().BeFalse();
            }
        }

        public class GameTurnEndsTests : GameTestsBase
        {
            [Fact]
            public void End_turn_passes_turn_to_next_player()
            {
                var nextPlayer = Substitute.For<IPlayer>();
                var gameSetup = Make.GameSetup.Build();
                _playerFactory.Create(gameSetup.Players).Returns(new List<IPlayer>
                {
                    Substitute.For<IPlayer>(),
                    nextPlayer
                });

                var sut = Create(gameSetup);
                sut.EndTurn();

                sut.CurrentPlayer.Should().Be(nextPlayer);
            }

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
}