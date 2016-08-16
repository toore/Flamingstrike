using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectInteractionState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IInteractionContext _interactionContext;
        private readonly IGame _game;

        public FortifySelectInteractionState(IInteractionContext interactionContext, IInteractionStateFactory interactionStateFactory, IGame game)
        {
            _interactionContext = interactionContext;
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
            _interactionContext.Set(fortifyMoveInteractionState);
        }
    }
}