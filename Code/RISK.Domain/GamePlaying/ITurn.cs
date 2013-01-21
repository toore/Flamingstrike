using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        void SelectArea(IAreaDefinition areaDefinition);
        void AttackArea(IAreaDefinition areaDefinition);

        bool PlayerShouldReceiveCardWhenTurnEnds();
    }
}