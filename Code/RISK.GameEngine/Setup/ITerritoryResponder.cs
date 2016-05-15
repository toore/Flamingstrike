using RISK.Core;

namespace RISK.GameEngine.Setup
{
    public interface ITerritoryResponder
    {
        IRegion ProcessRequest(ITerritoryRequestParameter territoryRequestParameter);
    }
}