using System;
using RISK.Application.Entities;

namespace RISK.Application.GamePlaying
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
            return territory.Occupant == Player;
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