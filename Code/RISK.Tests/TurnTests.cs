using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying;
using Rhino.Mocks;

namespace RISK.Tests
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
        private LazyReturnValue<IEnumerable<ILocation>> _locationConnections;
        private LazyReturnValue<IEnumerable<ILocation>> _otherLocationConnections;

        [SetUp]
        public void SetUp()
        {
            _currentPlayer = MockRepository.GenerateStub<IPlayer>();
            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _battleCalculator = MockRepository.GenerateStub<IBattleCalculator>();

            _turn = new Turn(_currentPlayer, _worldMap, _battleCalculator);

            _locationConnections = new LazyReturnValue<IEnumerable<ILocation>>(new List<ILocation>());
            _location = GenerateLocationStub(_locationConnections);
            _territory = GenerateTerritoryStub(_location, _currentPlayer);

            _otherLocationConnections = new LazyReturnValue<IEnumerable<ILocation>>(new List<ILocation>());
            _otherLocation = GenerateLocationStub(_otherLocationConnections);
            _otherPlayer = MockRepository.GenerateStub<IPlayer>();
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

            _battleCalculator.AssertWasNotCalled(x => x.Attack(null, null), x => x.IgnoreArguments());
        }

        [Test]
        public void Can_attack_when_territories_are_connected()
        {
            StubLocationIsConnectedWithOtherLocation();

            SelectAndAttack();

            _battleCalculator.AssertWasCalled(x => x.Attack(_territory, _otherTerritory));
        }

        [Test]
        public void Player_should_not_receive_a_card_when_attack_fails()
        {
            StubLocationIsConnectedWithOtherLocation();

            SelectAndAttack();

            _turn.PlayerShouldReceiveCardWhenTurnEnds().Should().BeFalse();
        }

        [Test]
        public void Player_should_receive_a_card_when_attack_succeeds()
        {
            StubLocationIsConnectedWithOtherLocation();
            _battleCalculator.Stub(x => x.Attack(_territory, _otherTerritory)).WhenCalled(x => _otherTerritory.Owner = _currentPlayer);

            SelectAndAttack();

            _turn.PlayerShouldReceiveCardWhenTurnEnds().Should().BeTrue();
        }

        [Test]
        public void Player_should_not_receive_a_card_when_turn_begins()
        {
            _turn.PlayerShouldReceiveCardWhenTurnEnds().Should().BeFalse();
        }

        private void StubLocationIsConnectedWithOtherLocation()
        {
            _locationConnections.Value = _otherLocation.AsList();
        }

        private void SelectAndAttack()
        {
            _turn.Select(_location);
            _turn.Attack(_otherLocation);
        }

        private static ILocation GenerateLocationStub(LazyReturnValue<IEnumerable<ILocation>> connections)
        {
            var location = MockRepository.GenerateStub<ILocation>();
            location.Stub(x => x.Connections).LazyReturnValue(connections);

            return location;
        }

        private ITerritory GenerateTerritoryStub(ILocation location, IPlayer owner)
        {
            var territory = MockRepository.GenerateStub<ITerritory>();
            territory.Stub(x => x.Location).Return(location);
            territory.Owner = owner;

            _worldMap.Stub(x => x.GetTerritory(location)).Return(territory);

            return territory;
        }
    }
}