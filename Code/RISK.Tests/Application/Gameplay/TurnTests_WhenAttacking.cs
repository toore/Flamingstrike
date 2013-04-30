using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Base.Extensions;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class TurnTests_WhenAttacking
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
        public void Cant_attack()
        {
            _turn.CanAttack(null).Should().BeFalse("no territory has been selected");
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
        public void Cant_attack_when_having_only_one_army()
        {
            LocationIsConnectedToOtherLocation();
            _territory.Armies = 1;
            _turn.Select(_location);

            _turn.CanAttack(_otherLocation).Should().BeFalse("there is only one army in location");
        }

        [Test]
        public void Can_attack_when_territories_are_connected()
        {
            LocationIsConnectedToOtherLocation();
            _territory.Armies = 2;

            SelectAndAttack();

            _battleCalculator.Received().Attack(_territory, _otherTerritory);
        }

        [Test]
        public void When_attack_succeeds_selected_territory_is_updated_to_occupied()
        {
            LocationIsConnectedToOtherLocation();
            _territory.Armies = 2;
            AttackSucceeds();

            SelectAndAttack();

            _turn.SelectedTerritory.Should().Be(_otherTerritory, "attack suceeded so army should be moved into territory");
        }

        [Test]
        public void Player_should_not_receive_a_card_when_attack_fails_and_turn_ends()
        {
            SelectAndAttack();
            EndTurn();

            _currentPlayer.DidNotReceive().AddCard(null);
        }

        [Test]
        public void Player_should_receive_a_card_when_attack_succeeds_and_turn_ends()
        {
            LocationIsConnectedToOtherLocation();
            _territory.Armies = 2;
            AttackSucceeds();

            SelectAndAttack();
            EndTurn();

            _currentPlayer.Received().AddCard(null);
        }

        private void AttackSucceeds()
        {
            _battleCalculator
                .When(x => x.Attack(_territory, _otherTerritory))
                .Do(x => _otherTerritory.AssignedPlayer = _currentPlayer);
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
            territory.AssignedPlayer = owner;

            _worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }

        private void EndTurn()
        {
            _turn.EndTurn();
        }
    }
}