using System;
using FluentAssertions;
using NSubstitute;
using Ploeh.AutoFixture.Xunit2;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Core;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class DraftArmiesGameStateTests
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IArmyModifier _armyModifier;

        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        private readonly IDeck _deck;
        private readonly GameData _gameData;

        public DraftArmiesGameStateTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _gameDataFactory = Substitute.For<IGameDataFactory>();
            _armyModifier = Substitute.For<IArmyModifier>();

            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Substitute.For<IPlayer>();
            _anotherPlayer = Substitute.For<IPlayer>();

            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();
            _territory.Region.Returns(_region);
            _territory.Player.Returns(_currentPlayer);
            _anotherTerritory.Region.Returns(_anotherRegion);

            _deck = Substitute.For<IDeck>();

            _gameData = Make.GameData
                .CurrentPlayer(_currentPlayer)
                .WithPlayer(_currentPlayer)
                .WithPlayer(_anotherPlayer)
                .WithTerritory(_territory)
                .WithTerritory(_anotherTerritory)
                .WithDeck(_deck)
                .Build();
        }

        [Fact]
        public void Can_place_draft_armies_for_occupied_territory()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_region).Should().BeTrue();
        }

        [Theory, AutoData]
        public void Gets_number_of_armies_to_draft(int numberOfArmiesToDraft)
        {
            var sut = Create(_gameData, numberOfArmiesToDraft);

            sut.GetNumberOfArmiesToDraft().Should().Be(numberOfArmiesToDraft);
        }

        [Fact]
        public void Can_not_place_draft_armies_for_another_territory()
        {
            var sut = Create(_gameData);

            sut.CanPlaceDraftArmies(_anotherRegion).Should().BeFalse();
        }

        [Fact]
        public void After_placing_draft_armies_continues_to_draft_armies()
        {
            var updatedTerritories = new ITerritory[] { Make.Territory.Build() };
            var newGameData = Make.GameData.Build();
            _armyModifier
                .PlaceDraftArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                .Returns(updatedTerritories);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(updatedTerritories),
                _deck)
                .Returns(newGameData);

            var sut = Create(_gameData, 3);
            sut.PlaceDraftArmies(_region, 2);

            _gameStateConductor.Received().ContinueToDraftArmies(newGameData, 1);
        }

        [Fact]
        public void After_placing_all_draft_armies_continues_with_attack_phase()
        {
            var updatedTerritories = new ITerritory[] { Make.Territory.Build() };
            var newGameData = Make.GameData.Build();
            _armyModifier
                .PlaceDraftArmies(Argx.IsEquivalentReadOnly(_territory, _anotherTerritory), _region, 2)
                .Returns(updatedTerritories);
            _gameDataFactory.Create(
                _currentPlayer,
                Argx.IsEquivalentReadOnly(_currentPlayer, _anotherPlayer),
                Argx.IsEquivalentReadOnly(updatedTerritories),
                _deck)
                .Returns(newGameData);

            var sut = Create(_gameData, 2);
            sut.PlaceDraftArmies(_region, 2);

            _gameStateConductor.Received().ContinueWithAttackPhase(newGameData, ConqueringAchievement.DoNotAwardCardAtEndOfTurn);
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

            sut.CanSendAdditionalArmiesToOccupy().Should().BeFalse();
        }

        [Fact]
        public void Sending_armies_to_occupy_throws()
        {
            var sut = Create(_gameData);

            Action act = () => sut.SendAdditionalArmiesToOccupy(1);

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

        private IGameState Create(GameData gameData, int numberOfArmiesToDraft = 1)
        {
            return new DraftArmiesGameState(_gameStateConductor, _gameDataFactory, _armyModifier, gameData, numberOfArmiesToDraft);
        }

        [Fact]
        public void Gets_current_player()
        {
            var currentPlayer = Substitute.For<IPlayer>();
            var gameData = Make.GameData
                .CurrentPlayer(currentPlayer)
                .Build();

            var sut = Create(gameData);

            sut.CurrentPlayer.Should().Be(currentPlayer);
        }
    }
}