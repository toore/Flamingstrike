using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectState : IInteractionState
    {
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifySelectState(IInteractionStateFactory interactionStateFactory)
        {
            _interactionStateFactory = interactionStateFactory;
        }

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            return stateController.Game.IsCurrentPlayerOccupyingTerritory(territory);
        }

        public void OnClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
        }

        //private readonly IStateController _stateController;
        //private readonly IInteractionStateFactory _interactionStateFactory;

        //public FortifySelectState(IStateController stateController, IInteractionStateFactory interactionStateFactory)
        //{
        //    _stateController = stateController;
        //    _interactionStateFactory = interactionStateFactory;
        //}

        //public ITerritory SelectedTerritory { get; }

        //public bool CanClick(ITerritory territory)
        //{
        //    //return territory.Occupant == PlayerId;
        //    return false;
        //}

        //public void OnClick(ITerritory territory)
        //{
        //    //if (territory.Occupant != PlayerId)
        //    //{
        //    //    throw new InvalidOperationException();
        //    //}

        //    _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, territory);
        //}
    }
}