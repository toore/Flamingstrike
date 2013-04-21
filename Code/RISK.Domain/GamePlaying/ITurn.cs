using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface ITurn
    {
        void Attack(ILocation location);
        bool CanSelect(ILocation location);
        void Select(ILocation location);
        bool IsTerritorySelected { get; }
        void EndTurn();
    }
}