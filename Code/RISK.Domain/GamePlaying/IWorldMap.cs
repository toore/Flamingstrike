using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IWorldMap
    {
        IArea GetArea(IAreaDefinition areaDefinition);
    }
}