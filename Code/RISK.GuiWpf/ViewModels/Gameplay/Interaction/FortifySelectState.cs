using System;
using RISK.Application;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectState : IInteractionState
    {
        private readonly IStateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifySelectState(IStateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            Player = player;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            //return territory.Occupant == PlayerId;
            return false;
        }

        public void OnClick(ITerritory territory)
        {
            //if (territory.Occupant != PlayerId)
            //{
            //    throw new InvalidOperationException();
            //}

            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, Player, territory);
        }
    }
}