using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IWorldMap
    {
        ITerritory GetArea(ITerritoryLocation territoryLocation);
    }
}