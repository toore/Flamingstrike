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