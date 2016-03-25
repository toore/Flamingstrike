using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application
{
    public abstract class GameTestsBase
    {
        protected readonly IGameState _draftArmiesGameState;
        protected readonly IGame _sut;

        protected GameTestsBase()
        {
            var newArmiesDraftCalculator = Substitute.For<IArmyDraftCalculator>();
            var gameStateFactory = Substitute.For<IGameStateFactory>();
            var gameFactory = new GameFactory(gameStateFactory, newArmiesDraftCalculator);

            _draftArmiesGameState = Substitute.For<IGameState>();
            gameStateFactory.CreateDraftArmiesGameState(null, 0).ReturnsForAnyArgs(_draftArmiesGameState);

            _sut = gameFactory.Create(Make.GamePlaySetup.Build());
        }
    }

    public class GameIsAsProxyTests : GameTestsBase
    {
        [Fact]
        public void Gets_current_player()
        {
            var player = Substitute.For<IPlayer>();
            _draftArmiesGameState.CurrentPlayer.Returns(player);

            _sut.CurrentPlayer.Should().Be(player);
        }

        [Fact]
        public void Gets_territory()
        {
            var region = Substitute.For<IRegion>();
            var territory = Substitute.For<ITerritory>();
            _draftArmiesGameState.GetTerritory(region).Returns(territory);

            _sut.GetTerritory(region).Should().Be(territory);
        }

        [Theory]
        [AutoData]
        public void Gets_number_of_armies_to_draft(int numberOfArmies)
        {
            _draftArmiesGameState.GetNumberOfArmiesToDraft().Returns(numberOfArmies);

            _sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_place_draft_armies(bool canPlaceArmies)
        {
            var region = Substitute.For<IRegion>();
            _draftArmiesGameState.CanPlaceDraftArmies(region).Returns(canPlaceArmies);

            _sut.CanPlaceDraftArmies(region).Should().Be(canPlaceArmies);
        }

        [Theory, AutoData]
        public void Places_draft_armies(int numberOfArmies)
        {
            var region = Substitute.For<IRegion>();

            _sut.PlaceDraftArmies(region, numberOfArmies);

            _draftArmiesGameState.Received().PlaceDraftArmies(region, numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_attack(bool canAttack)
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();
            _draftArmiesGameState.CanAttack(attackingRegion, defendingRegion).Returns(canAttack);

            _sut.CanAttack(attackingRegion, defendingRegion).Should().Be(canAttack);
        }

        [Fact]
        public void Attacks()
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();

            _sut.Attack(attackingRegion, defendingRegion);

            _draftArmiesGameState.Received().Attack(attackingRegion, defendingRegion);
        }

        [Theory, AutoData]
        public void Gets_number_of_armies_that_can_be_sent_to_occupy(int numberOfArmies)
        {
            _draftArmiesGameState.GetNumberOfArmiesThatCanBeSentToOccupy().Returns(numberOfArmies);

            _sut.GetNumberOfArmiesThatCanBeSentToOccupy().Should().Be(numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_send_armies_to_occupy(bool canSendInArmiesToOccupy)
        {
            _draftArmiesGameState.CanSendArmiesToOccupy().Returns(canSendInArmiesToOccupy);

            _sut.CanSendArmiesToOccupy().Should().Be(canSendInArmiesToOccupy);
        }

        [Theory, AutoData]
        public void Sends_armies_to_occupy(int numberOfArmies)
        {
            _sut.SendArmiesToOccupy(numberOfArmies);

            _draftArmiesGameState.Received().SendArmiesToOccupy(numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_fortify(bool canFortify)
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();
            _draftArmiesGameState.CanFortify(sourceRegion, destinationRegion).Returns(canFortify);

            _sut.CanFortify(sourceRegion, destinationRegion).Should().Be(canFortify);
        }

        [Fact]
        public void Fortifies()
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();

            _sut.Fortify(sourceRegion, destinationRegion, 1);

            _draftArmiesGameState.Received().Fortify(sourceRegion, destinationRegion, 1);
        }

        [Fact]
        public void Ends_turn()
        {
            _sut.EndTurn();

            _draftArmiesGameState.Received().EndTurn();
        }
    }

    public abstract class GameUpdatesGameStateAfter : GameTestsBase
    {
        private readonly IGameState _nextGameState;

        protected GameUpdatesGameStateAfter()
        {
            _nextGameState = Substitute.For<IGameState>();
        }

        public class PlacingDraftArmies : GameUpdatesGameStateAfter
        {
            public PlacingDraftArmies()
            {
                _draftArmiesGameState.PlaceDraftArmies(null, 0).ReturnsForAnyArgs(_nextGameState);
                _sut.PlaceDraftArmies(null, 0);
            }
        }

        public class Attacking : GameUpdatesGameStateAfter
        {
            public Attacking()
            {
                _draftArmiesGameState.Attack(null, null).ReturnsForAnyArgs(_nextGameState);
                _sut.Attack(null, null);
            }
        }

        public class SendingArmiesToOccupy : GameUpdatesGameStateAfter
        {
            public SendingArmiesToOccupy()
            {
                _draftArmiesGameState.SendArmiesToOccupy(0).ReturnsForAnyArgs(_nextGameState);
                _sut.SendArmiesToOccupy(0);
            }
        }

        public class Fortifying : GameUpdatesGameStateAfter
        {
            public Fortifying()
            {
                _draftArmiesGameState.Fortify(null, null, 1).ReturnsForAnyArgs(_nextGameState);
                _sut.Fortify(null, null, 1);
            }
        }

        public class EndingTurn : GameUpdatesGameStateAfter
        {
            public EndingTurn()
            {
                _draftArmiesGameState.EndTurn().ReturnsForAnyArgs(_nextGameState);
                _sut.EndTurn();
            }
        }

        [Fact]
        public void Place_draft_armies_use_new_game_state()
        {
            _sut.PlaceDraftArmies(null, 0);

            _nextGameState.ReceivedWithAnyArgs().PlaceDraftArmies(null, 0);
        }

        [Fact]
        public void Attack_use_new_game_state()
        {
            _sut.Attack(null, null);

            _nextGameState.ReceivedWithAnyArgs().Attack(null, null);
        }

        [Fact]
        public void Send_armies_to_occupy_use_new_game_state()
        {
            _sut.SendArmiesToOccupy(0);

            _nextGameState.ReceivedWithAnyArgs().SendArmiesToOccupy(0);
        }

        [Fact]
        public void Fortify_use_new_game_state()
        {
            _sut.Fortify(null, null, 1);

            _nextGameState.ReceivedWithAnyArgs().Fortify(null, null, 1);
        }

        [Fact]
        public void End_turn_use_new_game_state()
        {
            _sut.EndTurn();

            _nextGameState.ReceivedWithAnyArgs().EndTurn();
        }
    }
}