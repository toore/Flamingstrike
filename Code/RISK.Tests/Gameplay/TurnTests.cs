using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Gameplay
{
    [TestFixture]
    public class TurnTests
    {
        private Turn _turn;
        private IPlayer _currentPlayer;
        private IPlayer _otherPlayer;
        private IWorldMap _worldMap;
        private IBattleCalculator _battleCalculator;
        private ILocation _location;
        private ITerritory _territory;
        private ILocation _otherLocation;
        private ITerritory _otherTerritory;
        private ICardFactory _cardFactory;

        [SetUp]
        public void SetUp()
        {
            _currentPlayer = Substitute.For<IPlayer>();
            _worldMap = Substitute.For<IWorldMap>();
            _battleCalculator = Substitute.For<IBattleCalculator>();
            _cardFactory = Substitute.For<ICardFactory>();

            _turn = new Turn(_currentPlayer, _worldMap, _battleCalculator, _cardFactory);

            _location = Substitute.For<ILocation>();
            _territory = GenerateTerritoryStub(_location, _currentPlayer);

            _otherLocation = Substitute.For<ILocation>();
            _otherPlayer = Substitute.For<IPlayer>();
            _otherTerritory = GenerateTerritoryStub(_otherLocation, _otherPlayer);
        }

        [Test]
        public void Can_select_location()
        {
            _turn.CanSelect(_location).Should().BeTrue();
        }

        [Test]
        public void Can_not_select_other_location()
        {
            _turn.CanSelect(_otherLocation).Should().BeFalse();
        }

        [Test]
        public void SelectedTerritory_should_be_territory()
        {
            _turn.Select(_location);

            _turn.IsTerritorySelected.Should().BeTrue();
            _turn.SelectedTerritory.Should().Be(_territory);
        }

        [Test]
        public void No_territory_should_be_selected()
        {
            _turn.Select(_otherLocation);

            _turn.IsTerritorySelected.Should().BeFalse();
            _turn.SelectedTerritory.Should().BeNull();
        }

        [Test]
        public void Selecting_already_selected_deselects()
        {
            _turn.Select(_location);
            _turn.Select(_location);

            _turn.IsTerritorySelected.Should().BeFalse();
            _turn.SelectedTerritory.Should().BeNull();
        }

        [Test]
        public void Cant_attack_when_no_territory_is_selected()
        {
            Action attack = () => _turn.Attack(null);

            attack.ShouldNotThrow();
        }

        [Test]
        public void Cant_attack_when_territories_are_not_connected()
        {
            SelectAndAttack();

            _battleCalculator.DidNotReceiveWithAnyArgs().Attack(null, null);
        }

        [Test]
        public void Can_attack_when_territories_are_connected()
        {
            LocationIsConnectedToOtherLocation();

            SelectAndAttack();

            _battleCalculator.Received().Attack(_territory, _otherTerritory);
        }

        [Test]
        public void Player_should_not_receive_a_card_when_attack_fails()
        {
            SelectAndAttack();
            EndTurn();

            _currentPlayer.DidNotReceive().AddCard(null);
        }

        [Test]
        public void Player_should_receive_a_card_when_attack_succeeds()
        {
            LocationIsConnectedToOtherLocation();
            _battleCalculator.When(x => x.Attack(_territory, _otherTerritory)).Do(x => _otherTerritory.AssignedToPlayer = _currentPlayer);

            SelectAndAttack();
            EndTurn();

            _currentPlayer.Received().AddCard(null);
        }

        [Test]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            _turn.EndTurn();

            _currentPlayer.DidNotReceive().AddCard(null);
        }

        private void LocationIsConnectedToOtherLocation()
        {
            _location.Connections.Returns(_otherLocation.AsList());
        }

        private void SelectAndAttack()
        {
            _turn.Select(_location);
            _turn.Attack(_otherLocation);
        }

        private ITerritory GenerateTerritoryStub(ILocation location, IPlayer owner)
        {
            var territory = Substitute.For<ITerritory>();
            territory.Location.Returns(location);
            territory.AssignedToPlayer = owner;

            _worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }

        private void EndTurn()
        {
            _turn.EndTurn();
        }
    }
}