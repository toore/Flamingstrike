namespace FlamingStrike.GameEngine.Play
{
    public interface IGameObserver
    {
        void DraftArmies(IDraftArmiesPhase draftArmiesPhase);
        void Attack(IAttackPhase attackPhase);
        void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        void EndTurn(IEndTurnPhase endTurnPhase);
        void GameOver(IGameOverState gameOverState);
    }
}