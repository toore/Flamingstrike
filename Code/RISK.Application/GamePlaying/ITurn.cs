using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }

        void Attack(ILocation location);
        bool CanSelect(ILocation location);
        void Select(ILocation location);
        bool IsTerritorySelected { get; }
        void EndTurn();
    }
}