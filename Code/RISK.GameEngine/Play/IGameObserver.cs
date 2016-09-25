namespace RISK.GameEngine.Play
{
    public interface IGameObserver
    {
        void NewGame(IGame game);
        void DraftArmies(IDraftArmiesPhase draftArmiesPhase);
        void Attack(IAttackPhase attackPhase);
        void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase);
        void EndTurn(IEndTurnPhase endTurnPhase);
        void GameOver(IGameIsOver gameIsOver);
    }
}