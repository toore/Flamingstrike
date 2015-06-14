using RISK.Application.World;

namespace RISK.Application.GameSetup
{
    public interface ITerritoryRequestHandler
    {
        ITerritory ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}