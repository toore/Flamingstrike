using System;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class SelectState : IInteractionState
    {
        private readonly IStateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(IStateController stateController, IInteractionStateFactory interactionStateFactory, IPlayerId playerId)
        {
            PlayerId = playerId;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
        }

        public ITerritory SelectedTerritory
        {
            get { return null; }
        }

        public IPlayerId PlayerId { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            //return territory.Occupant == PlayerId;
            return false;
        }

        public void OnClick(ITerritory territory)
        {
            if (!CanClick(territory))
            {
                throw new InvalidOperationException();
            }

            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, PlayerId, territory);
        }
    }
}