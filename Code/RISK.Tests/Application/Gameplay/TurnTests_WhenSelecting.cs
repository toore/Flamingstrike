﻿using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class TurnTests_WhenSelecting
    {
        private Turn _turn;
        private IPlayer _currentPlayer;
        private IWorldMap _worldMap;
        private IBattleCalculator _battleCalculator;
        private ILocation _location;
        private ITerritory _territory;
        private ILocation _otherLocation;
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
        public void Can_not_select_when_territory_is_selected()
        {
            _turn.Select(_location);

            _turn.CanSelect(_otherLocation).Should().BeFalse("territory is already selected");
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

        private ITerritory GenerateTerritoryStub(ILocation location, IPlayer owner)
        {
            var territory = Substitute.For<ITerritory>();
            territory.Location.Returns(location);
            territory.Occupant = owner;

            _worldMap.GetTerritory(location).Returns(territory);

            return territory;
        }
    }
}