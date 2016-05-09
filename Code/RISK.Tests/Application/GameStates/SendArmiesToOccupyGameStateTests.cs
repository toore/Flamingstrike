using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using RISK.Application.Play.Planning;
using RISK.Application.World;
using RISK.Tests.Builders;
using RISK.Tests.Extensions;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class SendArmiesToOccupyGameStateTests : GameStateTestsBase
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IArmyModifier _armyModifier;
        private readonly ITerritory _attackingTerritory;
        private readonly ITerritory _occupiedTerritory;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;
        private readonly IPlayer _currentPlayer;
        private readonly IPlayer _anotherPlayer;
        //private readonly IDeck _deck;
        private readonly GameData _gameData;

        public SendArmiesToOccupyGameStateTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
            _armyModifier = Substitute.For<IArmyModifier>();

            _attackingTerritory = Substitute.For<ITerritory>();
            _occupiedTerritory = Substitute.For<ITerritory>();
            _currentPlayer = Make.Player.Build();
            _anotherPlayer = Make.Player.Build();

            _attackingRegion = Substitute.For<IRegion>();
            _occupiedRegion = Substitute.For<IRegion>();
            _attackingTerritory.Region.Returns(_attackingRegion);
            _attackingTerritory.Player.Returns(_currentPlayer);
            _occupiedTerritory.Region.Returns(_occupiedRegion);
            _occupiedTerritory.Player.Returns(_currentPlayer);

            //_region.HasBorder(_anotherRegion).Returns(true);
            //_territory.GetNumberOfArmiesAvailableForAttack().Returns(1);

            //_deck = Substitute.For<IDeck>();

            _gameData = Make.GameData
                .CurrentPlayer(_currentPlayer)
                .WithPlayer(_currentPlayer)
                .WithPlayer(_anotherPlayer)
                .WithTerritory(_attackingTerritory)
                .WithTerritory(_occupiedTerritory)
                //  .WithDeck(_deck)
                .Build();
        }

        [Fact]
        public void Can_not_place_draft_armies()
        {
            var sut = Create(null);

            sut.CanPlaceDraftArmies(null).Should().BeFalse();
        }

        [Fact]
        public void Get_number_of_armies_to_draft_throws()
        {
            var sut = Create(null);

            Action act = () => sut.GetNumberOfArmiesToDraft();

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Place_draft_armies_throws()
        {
            var sut = Create(null);

            Action act = () => sut.PlaceDraftArmies(null, 0);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_not_attack()
        {
            var sut = Create(null);

            sut.CanAttack(null, null).Should().BeFalse();
        }

        [Fact]
        public void Attack_throws()
        {
            var sut = Create(null);

            Action act = () => sut.Attack(null, null);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_send_additional_armies_to_occupy()
        {
            var sut = Create(_gameData);

            sut.CanSendAdditionalArmiesToOccupy().Should().BeTrue();
        }

        [Fact]
        public void Gets_the_number_of_additional_armies_that_can_be_sent_to_occupy()
        {
            _attackingTerritory.GetNumberOfArmiesThatCanBeSentToOccupy().Returns(1);

            var sut = Create(_gameData);

            sut.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy().Should().Be(1);
        }

        [Fact]
        public void Sending_armies_to_occupy_continues_with_attack_state()
        {
            var attackGameState = Substitute.For<IGameState>();

            IReadOnlyList<ITerritory> updatedTerritoriesAfterAdditionalArmiesHaveBeenSentToOccupy = new ITerritory[] { Make.Territory.Build(), Make.Territory.Build() };
            _armyModifier.SendInAdditionalArmiesToOccupy(
                Argx.IsEquivalentReadOnly(_attackingTerritory, _occupiedTerritory),
                _attackingRegion,
                _occupiedRegion,
                1)
                .Returns(updatedTerritoriesAfterAdditionalArmiesHaveBeenSentToOccupy);

            _gameStateConductor.ContinueWithAttackPhase(Arg.Is<GameData>(x =>
                x.CurrentPlayer == _currentPlayer
                &&
                x.Players.IsEquivalent(_currentPlayer, _anotherPlayer)
                &&
                x.Territories.IsEquivalent(updatedTerritoriesAfterAdditionalArmiesHaveBeenSentToOccupy)
                ),
                ConqueringAchievement.AwardCardAtEndOfTurn)
                .Returns(attackGameState);

            var sut = Create(_gameData);
            var gameState = sut.SendAdditionalArmiesToOccupy(1);

            gameState.Should().Be(attackGameState);
        }

        protected override IGameState Create(GameData gameData)
        {
            return new SendArmiesToOccupyGameState(_gameStateConductor, _armyModifier, gameData, _attackingRegion, _occupiedRegion);
        }
    }
}