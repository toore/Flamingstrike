using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectInteractionState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IGame _game;

        public FortifySelectInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game)
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

            EnterFortifyMoveState(_game, region);
        }

        private void EnterFortifyMoveState(IGame game, IRegion selectedRegion)
        {
            var fortifyMoveInteractionState = _interactionStateFactory.CreateFortifyMoveInteractionState(game, selectedRegion);
            _interactionStateFsm.Set(fortifyMoveInteractionState);
        }
    }
}