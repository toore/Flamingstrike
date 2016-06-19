using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class DraftArmiesInteractionState : IInteractionState
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;

        public DraftArmiesInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game)
        {
            _interactionStateFsm = interactionStateFsm;
            _interactionStateFactory = interactionStateFactory;
            _game = game;
        }

        public IRegion SelectedRegion => null;

        public bool CanClick(IRegion region)
        {
            return _game.IsCurrentPlayerOccupyingRegion(region)
                   &&
                   _game.GetNumberOfArmiesToDraft() > 0;
        }

        public void OnClick(IRegion region)
        {
            var numberOfArmies = 1;
            _game.PlaceDraftArmies(region, numberOfArmies);

            throw new NotImplementedException();
            if (_game.GetNumberOfArmiesToDraft() == 0)
            {
                //interactionStateFsm.CurrentState
            }
        }
    }
}