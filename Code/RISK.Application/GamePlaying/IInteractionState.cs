using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public interface IInteractionState
    {
        IPlayer Player { get; }
        ITerritory SelectedTerritory { get; }

        bool CanClick(ITerritory territory);
        void OnClick(ITerritory territory);
    }
}