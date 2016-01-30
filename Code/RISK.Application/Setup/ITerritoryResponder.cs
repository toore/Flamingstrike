using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryResponder
    {
        IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}