using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        void SelectArea(ITerritoryLocation territoryLocation);
        void AttackArea(ITerritoryLocation territoryLocation);

        bool PlayerShouldReceiveCardWhenTurnEnds();
    }
}