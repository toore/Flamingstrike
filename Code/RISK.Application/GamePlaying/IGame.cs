namespace RISK.Domain.GamePlaying
{
    public interface IGame
    {
        IWorldMap GetWorldMap();
        ITurnState GetNextTurn();
    }
}