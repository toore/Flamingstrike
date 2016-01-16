using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryResponder
    {
        ITerritoryGeography ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}