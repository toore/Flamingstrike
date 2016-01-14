using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryResponder
    {
        ITerritoryId ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}