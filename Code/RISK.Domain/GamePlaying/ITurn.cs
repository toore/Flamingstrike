using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        void SelectTerritory(ITerritoryLocation territoryLocation);
        void AttackTerritory(ITerritoryLocation territoryLocation);

        bool PlayerShouldReceiveCardWhenTurnEnds();
    }
}