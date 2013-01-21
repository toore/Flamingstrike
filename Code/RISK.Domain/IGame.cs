namespace RISK.Domain
{
    public interface IGame
    {
        IWorldMap GetWorldMap();
        ITurn GetNextTurn();
    }
}