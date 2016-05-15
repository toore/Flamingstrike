using RISK.Core;

namespace RISK.Application.Setup
{
    public interface ITerritoryResponder
    {
        IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}