namespace RISK.Domain.GamePlaying
{
    public interface IGame
    {
        IWorldMap GetWorldMap();
        ITurn GetNextTurn();
    }
}