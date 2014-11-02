namespace RISK.Domain.GamePlaying
{
    public interface IGame
    {
        IWorldMap WorldMap { get; }
        IInteractionState CurrentInteractionState { get; }
        void EndTurn();
        bool IsGameOver();
    }
}