using System;
using System.Collections.Generic;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Services.GameEngineClient;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public abstract class InteractionStateBase : IInteractionState
    {
        public abstract string Title { get; }

        public virtual bool CanEnterFortifyMode => false;

        public virtual bool CanEnterAttackMode => false;

        public virtual bool CanEndTurn => false;

        public virtual IReadOnlyList<Region> EnabledRegions => new List<Region>();

        public virtual Maybe<Region> SelectedRegion => Maybe<Region>.Nothing;

        public virtual bool CanUserSelectNumberOfArmies => false;

        public virtual int DefaultNumberOfUserSelectedArmies => 1;

        public virtual int MaxNumberOfUserSelectableArmies => 1;

        public virtual void OnRegionClicked(Region region)
        {
            throw new InvalidOperationException();
        }

        public virtual void EndTurn()
        {
            throw new InvalidOperationException();
        }
    }
}