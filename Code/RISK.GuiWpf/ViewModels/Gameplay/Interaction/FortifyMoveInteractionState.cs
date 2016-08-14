using System;
using RISK.Core;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifyMoveInteractionState : IInteractionState
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IGame _game;

        public FortifyMoveInteractionState(IInteractionStateFsm interactionStateFsm, IInteractionStateFactory interactionStateFactory, IGame game, IRegion selectedRegion)
        {
            _interactionStateFsm = interactionStateFsm;
            _interactionStateFactory = interactionStateFactory;
            _game = game;
            SelectedRegion = selectedRegion;
        }

        public IRegion SelectedRegion { get; }

        public bool CanClick(IRegion region)
        {
            return CanFortify(region)
                   ||
                   CanDeselect(region);
        }

        public void OnClick(IRegion region)
        {
            if (CanDeselect(region))
            {
                Deselect();
            }
            else if (CanFortify(region))
            {
                Fortify(region);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private bool CanDeselect(IRegion region)
        {
            return region == SelectedRegion;
        }

        private void Deselect()
        {
            EnterFortifySelectState();
        }

        private void EnterFortifySelectState()
        {
            var fortifySelectState = _interactionStateFactory.CreateFortifySelectInteractionState(_game);
            _interactionStateFsm.Set(fortifySelectState);
        }

        private bool CanFortify(IRegion regionToFortify)
        {
            var canFortify = _game.CanFortify(SelectedRegion, regionToFortify);
            return canFortify;
        }

        private void Fortify(IRegion regionToFortify)
        {
            _game.Fortify(SelectedRegion, regionToFortify, 1);

            EnterEndTurnState();
        }

        private void EnterEndTurnState()
        {
            var endTurnState = _interactionStateFactory.CreateEndTurnInteractionState();
            _interactionStateFsm.Set(endTurnState);
        }
    }
}