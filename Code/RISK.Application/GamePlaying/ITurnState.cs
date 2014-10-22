using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurnState
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }
        bool IsTerritorySelected { get; }

        bool CanSelect(ILocation location);
        void Select(ILocation location);
        bool CanAttack(ILocation location);
        void Attack(ILocation location);
        bool IsFortificationAllowedInTurn();
        bool CanFortify(ILocation location);
        void Fortify(ILocation location, int armies);
        void EndTurn();
    }
}