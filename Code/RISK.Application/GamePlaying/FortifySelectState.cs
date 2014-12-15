using System;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
{
    public class FortifySelectState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public FortifySelectState(StateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
        {
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            Player = player;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool CanClick(ITerritory territory)
        {
            return territory.Occupant == Player;
        }

        public void OnClick(ITerritory territory)
        {
            if (territory.Occupant != Player)
            {
                throw new InvalidOperationException();
            }

            _stateController.CurrentState = _interactionStateFactory.CreateFortifyState(_stateController, Player, territory);
        }
    }
}