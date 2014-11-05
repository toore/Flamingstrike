namespace RISK.Application.GamePlaying
{
    public interface IGame
    {
        IWorldMap WorldMap { get; }
        IInteractionState CurrentInteractionState { get; }
        void EndTurn();
        bool IsGameOver();
    }
}