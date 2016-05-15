using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.GameEngine.Setup;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application
{
    public class GameTests
    {
        private readonly GameFactory _factory;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IDeckFactory _deckFactory;
        private readonly IGameStateFsm _gameStateFsm;

        public GameTests()
        {
            _gameDataFactory = Substitute.For<IGameDataFactory>();
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _deckFactory = Substitute.For<IDeckFactory>();
            _gameStateFsm = Substitute.For<IGameStateFsm>();

            _factory = new GameFactory(_gameDataFactory, _gameStateConductor, _deckFactory, _gameStateFsm);
        }

        [Theory]
        [AutoData]
        public void Game_initializes_draft_armies_game_state()
        {
            var territory = Substitute.For<ITerritory>();
            var anotherTerritory = Substitute.For<ITerritory>();
            var firstPlayer = Substitute.For<IPlayer>();
            var secondPlayer = Substitute.For<IPlayer>();
            var thirdPlayer = Substitute.For<IPlayer>();
            var deck = Substitute.For<IDeck>();
            _deckFactory.Create().Returns(deck);
            var gameData = Make.GameData.Build();
            var gamePlaySetup = Make.GamePlaySetup
                .WithTerritory(territory)
                .WithTerritory(anotherTerritory)
                .WithPlayer(firstPlayer)
                .WithPlayer(secondPlayer)
                .WithPlayer(thirdPlayer)
                .Build();
            _gameDataFactory.Create(
                firstPlayer,
                Argx.IsEquivalentReadOnly(firstPlayer, secondPlayer, thirdPlayer),
                Argx.IsEquivalentReadOnly(territory, anotherTerritory),
                deck)
                .Returns(gameData);

            Create(gamePlaySetup);

            _gameStateConductor.Received(1).InitializeFirstPlayerTurn(gameData);
        }

        [Fact]
        public void Gets_current_player()
        {
            var player = Substitute.For<IPlayer>();
            _gameStateFsm.CurrentPlayer.Returns(player);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.CurrentPlayer.Should().Be(player);
        }

        [Fact]
        public void Gets_territory()
        {
            var region = Substitute.For<IRegion>();
            var territory = Substitute.For<ITerritory>();
            _gameStateFsm.GetTerritory(region).Returns(territory);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.GetTerritory(region).Should().Be(territory);
        }

        [Theory, AutoData]
        public void Can_place_draft_armies(bool canPlaceArmies)
        {
            var region = Substitute.For<IRegion>();
            _gameStateFsm.CanPlaceDraftArmies(region).Returns(canPlaceArmies);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.CanPlaceDraftArmies(region).Should().Be(canPlaceArmies);
        }

        [Theory]
        [AutoData]
        public void Gets_number_of_armies_to_draft(int numberOfArmies)
        {
            _gameStateFsm.GetNumberOfArmiesToDraft().Returns(numberOfArmies);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmies);
        }

        [Theory, AutoData]
        public void Places_draft_armies(int numberOfArmies)
        {
            var region = Substitute.For<IRegion>();

            var sut = Create(Make.GamePlaySetup.Build());
            sut.PlaceDraftArmies(region, numberOfArmies);

            _gameStateFsm.Received().PlaceDraftArmies(region, numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_attack(bool canAttack)
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();
            _gameStateFsm.CanAttack(attackingRegion, defendingRegion).Returns(canAttack);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.CanAttack(attackingRegion, defendingRegion).Should().Be(canAttack);
        }

        [Fact]
        public void Attacks()
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();

            var sut = Create(Make.GamePlaySetup.Build());
            sut.Attack(attackingRegion, defendingRegion);

            _gameStateFsm.Received().Attack(attackingRegion, defendingRegion);
        }

        [Theory, AutoData]
        public void Gets_number_of_armies_that_can_be_sent_to_occupy(int numberOfArmies)
        {
            _gameStateFsm.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy().Returns(numberOfArmies);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.GetNumberOfArmiesThatCanBeSentToOccupy().Should().Be(numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_send_additional_armies_to_occupy(bool canSendInArmiesToOccupy)
        {
            _gameStateFsm.CanSendAdditionalArmiesToOccupy().Returns(canSendInArmiesToOccupy);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.CanSendArmiesToOccupy().Should().Be(canSendInArmiesToOccupy);
        }

        [Theory, AutoData]
        public void Sends_armies_to_occupy(int numberOfArmies)
        {
            var sut = Create(Make.GamePlaySetup.Build());
            sut.SendArmiesToOccupy(numberOfArmies);

            _gameStateFsm.Received().SendAdditionalArmiesToOccupy(numberOfArmies);
        }

        [Theory, AutoData]
        public void Can_fortify(bool canFortify)
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();
            _gameStateFsm.CanFortify(sourceRegion, destinationRegion).Returns(canFortify);

            var sut = Create(Make.GamePlaySetup.Build());

            sut.CanFortify(sourceRegion, destinationRegion).Should().Be(canFortify);
        }

        [Theory, AutoData]
        public void Fortifies(int armies)
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();

            var sut = Create(Make.GamePlaySetup.Build());
            sut.Fortify(sourceRegion, destinationRegion, armies);

            _gameStateFsm.Received().Fortify(sourceRegion, destinationRegion, armies);
        }

        [Fact]
        public void Ends_turn()
        {
            var sut = Create(Make.GamePlaySetup.Build());
            sut.EndTurn();

            _gameStateFsm.Received().EndTurn();
        }

        private IGame Create(IGamePlaySetup gamePlaySetup)
        {
            return _factory.Create(gamePlaySetup);
        }
    }
}