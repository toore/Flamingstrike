using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }
        bool IsTerritorySelected { get; }

        bool CanSelect(ILocation location);
        void Select(ILocation location);
        bool CanAttack(ITerritory territoryToAttack);
        void Attack(ILocation location);
        void EndTurn();
    }
}