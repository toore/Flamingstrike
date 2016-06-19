using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public abstract class SelectInteractionStateBase : IInteractionState
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IGame _game;

        protected SelectInteractionStateBase(IInteractionStateFsm interactionStateFsm, IGame game)
        {
            _interactionStateFsm = interactionStateFsm;
            _game = game;
        }

        public IRegion SelectedRegion => null;

        public bool CanClick(IRegion region)
        {
            return _game.IsCurrentPlayerOccupyingRegion(region);
        }

        public void OnClick(IRegion region)
        {
            if (!CanClick(region))
            {
                throw new InvalidOperationException();
            }

            var stateToEnterWhenSelected = CreateStateToEnterWhenSelected(_game, region);
            _interactionStateFsm.Set(stateToEnterWhenSelected);
        }

        protected abstract IInteractionState CreateStateToEnterWhenSelected(IGame game, IRegion selectedRegion);
    }
}