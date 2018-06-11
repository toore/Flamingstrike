using System;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using IAttackPhase = FlamingStrike.GameEngine.Play.IAttackPhase;
using IDraftArmiesPhase = FlamingStrike.GameEngine.Play.IDraftArmiesPhase;
using IEndTurnPhase = FlamingStrike.GameEngine.Play.IEndTurnPhase;
using IGameOverState = FlamingStrike.GameEngine.Play.IGameOverState;
using ISendArmiesToOccupyPhase = FlamingStrike.GameEngine.Play.ISendArmiesToOccupyPhase;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public class GameObserverAdapter : GameEngine.Play.IGameObserver
    {
        private readonly IObserver<Play.IDraftArmiesPhase> _draftArmiesPhaseObserver;
        private readonly IObserver<Play.IAttackPhase> _attackPhaseObserver;
        private readonly IObserver<Play.ISendArmiesToOccupyPhase> _sendArmiesToOccupyPhaseObserver;
        private readonly IObserver<Play.IEndTurnPhase> _endTurnPhaseObserver;
        private readonly IObserver<Play.IGameOverState> _gameOverStateObserver;

        public GameObserverAdapter(
            IObserver<Play.IDraftArmiesPhase> draftArmiesPhaseObserver,
            IObserver<Play.IAttackPhase> attackPhaseObserver,
            IObserver<Play.ISendArmiesToOccupyPhase> sendArmiesToOccupyPhaseObserver,
            IObserver<Play.IEndTurnPhase> endTurnPhaseObserver,
            IObserver<Play.IGameOverState> gameOverStateObserver)
        {
            _draftArmiesPhaseObserver = draftArmiesPhaseObserver;
            _attackPhaseObserver = attackPhaseObserver;
            _sendArmiesToOccupyPhaseObserver = sendArmiesToOccupyPhaseObserver;
            _endTurnPhaseObserver = endTurnPhaseObserver;
            _gameOverStateObserver = gameOverStateObserver;
        }

        public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
        {
            _draftArmiesPhaseObserver.OnNext(new DraftArmiesPhaseAdapter(draftArmiesPhase));
        }

        public void Attack(IAttackPhase attackPhase)
        {
            _attackPhaseObserver.OnNext(new AttackPhaseAdapter(attackPhase));
        }

        public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
        {
            _sendArmiesToOccupyPhaseObserver.OnNext(new SendArmiesToOccupyPhaseAdapter(sendArmiesToOccupyPhase));
        }

        public void EndTurn(IEndTurnPhase endTurnPhase)
        {
            _endTurnPhaseObserver.OnNext(new EndTurnPhaseAdapter(endTurnPhase));
        }

        public void GameOver(IGameOverState gameOverState)
        {
            _gameOverStateObserver.OnNext(new GameOverState((string)gameOverState.Winner));
        }
    }
}