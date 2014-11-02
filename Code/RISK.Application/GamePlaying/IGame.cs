namespace RISK.Domain.GamePlaying
{
    public interface IGame
    {
        IWorldMap WorldMap { get; }
        IInteractionState CurrentTurn { get; }
        void EndTurn();
        bool IsGameOver();
    }
}