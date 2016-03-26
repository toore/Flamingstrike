using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class DraftArmiesGameStateTests : GameStateTestsBase
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly ITerritoryUpdater _territoryUpdater;

        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly GameData _gameData;

        public DraftArmiesGameStateTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
            _territoryUpdater = Substitute.For<ITerritoryUpdater>();

            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Substitute.For<IPlayer>();
            _anotherPlayer = Substitute.For<IPlayer>();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);

            _gameData = Make.GameData
                .CurrentPlayer(_currentPlayer)
                .WithPlayer(_currentPlayer)
                .WithPlayer(_anotherPlayer)
                .WithTerritory(_territory)
                .WithTerritory(_anotherTerritory)
                .Build();
        }

        [Theory, AutoData]
        public void Gets_number_of_armies_to_draft(int numberOfArmiesToDraft)
        {
            var sut = Create(_gameData, numberOfArmiesToDraft);

            sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmiesToDraft);
        }

        [Fact]
        public void Can_place_draft_armies_for_occupied_territory()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_region).Should().BeTrue();
        }

        [Fact]
        public void Can_not_place_draft_armies_for_another_territory()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Placing_draft_armies_returns_draft_armies_game_state()
        {
            var updatedTerritories = new List<ITerritory> { Make.Territory.Build() };
            var draftArmiesGameState = Substitute.For<IGameState>();
            _territoryUpdater
                .PlaceArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                .Returns(updatedTerritories);
            _gameStateFactory.CreateDraftArmiesGameState(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(updatedTerritories)
                ),
                1)
                .Returns(draftArmiesGameState);

            var sut = Create(_gameData, 3);
            var result = sut.PlaceDraftArmies(_region, 2);

            result.Should().Be(draftArmiesGameState);
        }

        [Fact]
        public void Placing_all_draft_armies_returns_attack_game_state()
        {
            var updatedTerritories = new List<ITerritory> { Make.Territory.Build() };
            var attackGameState = Substitute.For<IGameState>();
            _territoryUpdater
                .PlaceArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                .Returns(updatedTerritories);
            _gameStateFactory.CreateAttackGameState(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(updatedTerritories)))
                .Returns(attackGameState);

            var sut = Create(_gameData, 2);
            var result = sut.PlaceDraftArmies(_region, 2);

            result.Should().Be(attackGameState);
        }

        [Fact]
        public void Placing_more_draft_armies_than_left_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.PlaceDraftArmies(_region, 2);

            act.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Can_not_attack()
        {
            var sut = Create(_gameData);

            sut.CanAttack(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Attack_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.Attack(_region, _anotherRegion);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_not_send_armies_to_occupy()
        {
            var sut = Create(_gameData);

            sut.CanSendArmiesToOccupy().Should().BeFalse();
        }

        [Fact]
        public void Sending_armies_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.SendArmiesToOccupy(1);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_not_fortify()
        {
            var sut = Create(_gameData);

            sut.CanFortify(_region, _anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void Fortifying_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.Fortify(_region, _anotherRegion, 1);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_not_end_turn()
        {
            var sut = Create(_gameData);

            sut.CanEndTurn().Should().BeFalse();
        }

        [Fact]
        public void End_turn_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.EndTurn();

            act.ShouldThrow<InvalidOperationException>();
        }

        protected override IGameState Create(GameData gameData)
        {
            return Create(gameData, 1);
        }

        private IGameState Create(GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(_gameStateFactory, _territoryUpdater, gameData, numberOfArmiesToDraft);
        }
    }
}