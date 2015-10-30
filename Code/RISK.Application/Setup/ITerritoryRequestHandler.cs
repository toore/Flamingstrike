using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestHandler
    {
        ITerritoryId ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}