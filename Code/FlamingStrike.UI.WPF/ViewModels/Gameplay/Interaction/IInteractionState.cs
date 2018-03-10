using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        string Title { get; }
        bool CanEnterFortifyMode { get; }
        bool CanEnterAttackMode { get; }
        bool CanEndTurn { get; }
        void OnRegionClicked(IRegion region);
        void EndTurn();
    }
}