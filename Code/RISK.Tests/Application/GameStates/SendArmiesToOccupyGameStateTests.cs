using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;
using Xunit;

namespace RISK.Tests.Application.GameStates
{
    public class SendArmiesToOccupyGameStateTests : GameStateTestsBase
    {
        private readonly IGameStateConductor _gameStateConductor;

        public SendArmiesToOccupyGameStateTests()
        {
            _gameStateConductor = Substitute.For<IGameStateConductor>();
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
        public void Can_send_armies_to_occupy()
        {
            var sut = Create(null);

            sut.CanSendArmiesToOccupy().Should().BeTrue();
        }

        [Fact(Skip = "Not implemented")]
        public void Can_move_armies_into_captured_territory()
        {
            //var defenderIsDefeated = Substitute.For<IBattleResult>();
            //defenderIsDefeated.IsDefenderDefeated().Returns(true);
            ////GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //_battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //    .Returns(defenderIsDefeated);

            //var sut = Create(_gamePlaySetup);
            //sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);

            //sut.CanSendArmiesToOccupy().Should().BeTrue();
        }

        [Fact(Skip = "Not implemented")]
        public void Moves_armies_into_captured_territory()
        {
            //var defenderIsEliminated = Substitute.For<IBattleResult>();
            //defenderIsEliminated.IsDefenderDefeated().Returns(true);
            ////GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //_battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //    .Returns(defenderIsEliminated);

            //var sut = Create(_gamePlaySetup);
            //sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);
            //sut.SendArmiesToOccupy(3);

            //// Move to own test fixture
            //// Test that canmove 
            //// test move
            //// (test to attack and standard move)
            //// TBD: test that canmove prevents other actions
        }

        [Fact(Skip = "Not implemented")]
        public void Can_not_attack_before_move_into_captured_territory_has_been_confirmed()
        {
            //var defenderIsEliminated = Substitute.For<IBattleResult>();
            //defenderIsEliminated.IsDefenderDefeated().Returns(true);
            ////GetAttackCandidatesReturns(_anotherPlayerTerritory);
            //_battle.Attack(_currentPlayerTerritory, _anotherPlayerTerritory)
            //    .Returns(defenderIsEliminated);

            //var sut = Create(_gamePlaySetup);
            //sut.Attack(_currentPlayerRegion, _anotherPlayerRegion);

            //sut.AssertCanNotAttack(_currentPlayerRegion, _anotherPlayerRegion);
        }

        protected override IGameState Create(GameData gameData)
        {
            return new SendArmiesToOccupyGameState(_gameStateConductor, gameData, ConqueringAchievement.DoNotAwardCardAtEndOfTurn);
        }
    }
}