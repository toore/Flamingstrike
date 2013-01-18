using RISK.Domain.Entities;

namespace RISK.Domain
{
    public interface ITurn
    {
        void SelectArea(IAreaDefinition areaDefinition);
        void AttackArea(IAreaDefinition areaDefinition);
    }
}