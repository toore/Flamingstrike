using System.Collections.Generic;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public interface IInteractionState
    {
        string Title { get; }
        bool CanEnterFortifyMode { get; }
        bool CanEnterAttackMode { get; }
        bool CanEndTurn { get; }
        IReadOnlyList<Region> EnabledRegions { get; }
        Maybe<Region> SelectedRegion { get; }
        bool CanUserSelectNumberOfArmies { get; }
        int DefaultNumberOfUserSelectedArmies { get; }
        int MaxNumberOfUserSelectableArmies { get; }
        void OnRegionClicked(Region region);
        void EndTurn();
    }
}