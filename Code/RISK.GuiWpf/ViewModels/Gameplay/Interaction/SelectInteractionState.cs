using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectInteractionState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IGame _game;

        public SelectInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game)
        {
            _interactionStateFsm = interactionStateFsm;
            _interactionStateFactory = interactionStateFactory;
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

            EnterAttackInteractionState(_game, region);
        }

        private void EnterAttackInteractionState(IGame game, IRegion selectedRegion)
        {
            var attackInteractionState = _interactionStateFactory.CreateAttackInteractionState(game, selectedRegion);
            _interactionStateFsm.Set(attackInteractionState);
        }
    }
}