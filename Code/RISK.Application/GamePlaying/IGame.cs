namespace RISK.Application.GamePlaying
{
    public interface IGame
    {
        Territories Territories { get; }
        IInteractionState CurrentInteractionState { get; }
        void EndTurn();
        bool IsGameOver();
    }
}