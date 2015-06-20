using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestHandler
    {
        ITerritory ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}