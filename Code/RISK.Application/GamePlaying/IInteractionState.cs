using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public interface IInteractionState
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }

        bool CanClick(ILocation location);
        void OnClick(ILocation location);


        void EndTurn(); // TO BE REMOVED
    }
}