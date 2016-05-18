using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Core;
using RISK.GameEngine.Play.GamePhases;
using RISK.Tests.Builders;
using Xunit;
using IPlayer = RISK.GameEngine.Play.IPlayer;

namespace RISK.Tests.GameEngine
{
    public class GameStateFsmTests
    {
        private readonly GameStateFsm _sut;
        private readonly IGameState _gameState;

        public GameStateFsmTests()
        {
            _sut = new GameStateFsm();

            _gameState = Substitute.For<IGameState>();
            _sut.Set(_gameState);
        }

        [Fact]
        public void Gets_current_player()
        {
            var currentPlayer = Substitute.For<IPlayer>();
            _gameState.CurrentPlayer.Returns(currentPlayer);

            _sut.CurrentPlayer.Should().Be(currentPlayer);
        }

        [Fact]
        public void Gets_territory()
        {
            var region = Substitute.For<IRegion>();
            var territory = Make.Territory.Region(region).Build();
            _gameState.Territories.Returns(new[] { Make.Territory.Build(), territory });

            _sut.GetTerritory(region).Should().Be(territory);
        }

        [Theory, AutoData]
        public void Gets_can_place_draft_armies(bool canPlaceDraftArmies)
        {
            var region = Substitute.For<IRegion>();

            _gameState.CanPlaceDraftArmies(region).Returns(canPlaceDraftArmies);

            _sut.CanPlaceDraftArmies(region).Should().Be(canPlaceDraftArmies);
        }

        [Theory, AutoData]
        public void Gets_number_of_armies_to_draft(int numberOfArmiesToDraft)
        {
            _gameState.GetNumberOfArmiesToDraft().Returns(numberOfArmiesToDraft);

            _sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmiesToDraft);
        }

        [Theory, AutoData]
        public void Places_draft_armies(int numberOfArmies)
        {
            var region = Substitute.For<IRegion>();

            _sut.PlaceDraftArmies(region, numberOfArmies);

            _gameState.Received().PlaceDraftArmies(region, numberOfArmies);
        }

        [Theory, AutoData]
        public void Gets_can_attack(bool canAttack)
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();

            _gameState.CanAttack(attackingRegion, defendingRegion).Returns(canAttack);

            _sut.CanAttack(attackingRegion, defendingRegion).Should().Be(canAttack);
        }

        [Theory, AutoData]
        public void Attacks()
        {
            var attackingRegion = Substitute.For<IRegion>();
            var defendingRegion = Substitute.For<IRegion>();

            _sut.Attack(attackingRegion, defendingRegion);

            _gameState.Received().Attack(attackingRegion, defendingRegion);
        }

        [Theory, AutoData]
        public void Gets_can_send_additional_armies_to_occupy(bool canSendAdditionalArmiesToOccupy)
        {
            _gameState.CanSendAdditionalArmiesToOccupy().Returns(canSendAdditionalArmiesToOccupy);

            _sut.CanSendAdditionalArmiesToOccupy().Should().Be(canSendAdditionalArmiesToOccupy);
        }

        [Theory, AutoData]
        public void Gets_numbers_of_additional_armies_that_can_be_sent_to_occupy(int additionalArmiesThatCanBeSentToOccupy)
        {
            _gameState.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy().Returns(additionalArmiesThatCanBeSentToOccupy);

            _sut.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy().Should().Be(additionalArmiesThatCanBeSentToOccupy);
        }

        [Theory, AutoData]
        public void Sends_additional_armies_to_occupy(int numberOfArmies)
        {
            _sut.SendAdditionalArmiesToOccupy(numberOfArmies);

            _gameState.Received().SendAdditionalArmiesToOccupy(numberOfArmies);
        }

        [Theory, AutoData]
        public void Gets_can_fortify(bool canFortify)
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();

            _gameState.CanFortify(sourceRegion, destinationRegion).Returns(canFortify);

            _sut.CanFortify(sourceRegion, destinationRegion).Should().Be(canFortify);
        }

        [Theory, AutoData]
        public void Fortifies(int armies)
        {
            var sourceRegion = Substitute.For<IRegion>();
            var destinationRegion = Substitute.For<IRegion>();

            _sut.Fortify(sourceRegion, destinationRegion, armies);

            _gameState.Received().Fortify(sourceRegion, destinationRegion, armies);
        }

        [Fact]
        public void Ends_turn()
        {
            _sut.EndTurn();

            _gameState.Received().EndTurn();
        }
    }
}