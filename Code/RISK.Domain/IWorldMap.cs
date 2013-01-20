using RISK.Domain.Entities;

namespace RISK.Domain
{
    public interface IWorldMap
    {
        IArea GetArea(IAreaDefinition areaDefinition);
    }
}