namespace RISK.Domain.GamePlaying
{
    public interface IGame
    {
        Territories Territories { get; }
        IInteractionState CurrentInteractionState { get; }
        void EndTurn();
        bool IsGameOver();
    }
}