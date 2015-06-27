using System;
using RISK.Application.World;

namespace GuiWpf.ViewModels.Gameplay.Interaction
{
    public class FortifySelectState : IInteractionState
    {
        public FortifySelectState(InteractionStateFactory interactionStateFactory)
        {
            throw new NotImplementedException();
        }

        public bool CanClick(IStateController stateController, ITerritory territory)
        {
            throw new NotImplementedException();
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