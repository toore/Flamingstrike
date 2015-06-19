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

        public SelectState(IStateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
        }

        public ITerritory SelectedTerritory
        {
            get { return null; }
        }

        public IPlayer Player { get; private set; }

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

            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, territory);
        }
    }
}