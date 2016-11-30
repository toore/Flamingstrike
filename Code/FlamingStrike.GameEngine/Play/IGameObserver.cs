namespace FlamingStrike.GameEngine.Play
{
    public interface IGameObserver
    {
        void DraftArmies(IGameStatus gameStatus, IDraftArmiesPhase draftArmiesPhase);
        void Attack(IGameStatus gameStatus, IAttackPhase attackPhase);
        void SendArmiesToOccupy(IGameStatus gameStatus, ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        void EndTurn(IGameStatus gameStatus, IEndTurnPhase endTurnPhase);
        void GameOver(IGameOverState gameOverState);
    }
}