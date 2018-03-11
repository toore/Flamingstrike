using System.Collections.Generic;
using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        string Title { get; }
        bool CanEnterFortifyMode { get; }
        bool CanEnterAttackMode { get; }
        bool CanEndTurn { get; }
        IReadOnlyList<IRegion> EnabledRegions { get; }
        Maybe<IRegion> SelectedRegion { get; }
        void OnRegionClicked(IRegion region);
        void EndTurn();
    }
}