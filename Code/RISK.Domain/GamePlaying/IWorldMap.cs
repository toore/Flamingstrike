using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IWorldMap
    {
        ITerritory GetTerritory(ILocation location);
    }
}