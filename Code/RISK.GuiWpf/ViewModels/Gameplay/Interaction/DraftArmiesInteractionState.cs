using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesInteractionState : IInteractionState
    {
        private readonly IInteractionContext _interactionContext;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;

        public DraftArmiesInteractionState(IInteractionContext interactionContext, IInteractionStateFactory interactionStateFactory, IGame game)
        {
            _interactionContext = interactionContext;
            _interactionStateFactory = interactionStateFactory;
            _game = game;
        }

        public IRegion SelectedRegion => null;

        public bool CanClick(IRegion region)
        {
            return _game.IsCurrentPlayerOccupyingRegion(region)
                   &&
                   _game.HasArmiesToDraft();
        }

        public void OnClick(IRegion region)
        {
            if (!CanClick(region))
            {
                throw new InvalidOperationException();    
            }

            var numberOfArmies = 1;
            _game.PlaceDraftArmies(region, numberOfArmies);

            if (!_game.HasArmiesToDraft())
            {
                EnterSelectState();
            }
        }

        private void EnterSelectState()
        {
            var selectInteractionState = _interactionStateFactory.CreateSelectInteractionState(_game);
            _interactionContext.Set(selectInteractionState);
        }
    }
}