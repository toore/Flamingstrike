using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using IAttackPhase = FlamingStrike.GameEngine.Play.IAttackPhase;
using IDraftArmiesPhase = FlamingStrike.GameEngine.Play.IDraftArmiesPhase;
using IEndTurnPhase = FlamingStrike.GameEngine.Play.IEndTurnPhase;
using IGameOverState = FlamingStrike.GameEngine.Play.IGameOverState;
using ISendArmiesToOccupyPhase = FlamingStrike.GameEngine.Play.ISendArmiesToOccupyPhase;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient
{
    public partial class GameEngineAdapter
    {
        private class GameObserverAdapter : GameEngine.Play.IGameObserver
        {
            private readonly IGameObserver _gameObserver;

            public GameObserverAdapter(IGameObserver gameObserver)
            {
                _gameObserver = gameObserver;
            }

            public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
            {
                _gameObserver.DraftArmies(new DraftArmiesPhaseAdapter(draftArmiesPhase));
            }

            public void Attack(IAttackPhase attackPhase)
            {
                _gameObserver.Attack(new AttackPhaseAdapter(attackPhase));
            }

            public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
            {
                _gameObserver.SendArmiesToOccupy(new SendArmiesToOccupyPhaseAdapter(sendArmiesToOccupyPhase));
            }

            public void EndTurn(IEndTurnPhase endTurnPhase)
            {
                _gameObserver.EndTurn(new EndTurnPhaseAdapter(endTurnPhase));
            }

            public void GameOver(IGameOverState gameOverState)
            {
                _gameObserver.GameOver(new GameOverState((string)gameOverState.Winner));
            }
        }
    }
}