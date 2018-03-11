using System;
using System.Collections.Generic;
using FlamingStrike.GameEngine;

namespace FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction
{
    public abstract class InteractionStateBase : IInteractionState
    {
        public abstract string Title { get; }

        public virtual bool CanEnterFortifyMode => false;

        public virtual bool CanEnterAttackMode => false;

        public virtual bool CanEndTurn => false;

        public virtual IReadOnlyList<IRegion> EnabledRegions => new List<IRegion>();

        public virtual Maybe<IRegion> SelectedRegion => Maybe<IRegion>.Nothing;

        public virtual void OnRegionClicked(IRegion region)
        {
            throw new InvalidOperationException();
        }

        public virtual void EndTurn()
        {
            throw new InvalidOperationException();
        }
    }
}