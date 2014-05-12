using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }
        bool IsTerritorySelected { get; }

        bool CanSelect(ILocation location);
        bool CanAttack(ILocation location);
        bool CanFortify(ILocation location);
        void Select(ILocation location);
        void Attack(ILocation location);
        void Fortify(ILocation location, int armies);
        void EndTurn();
    }
}