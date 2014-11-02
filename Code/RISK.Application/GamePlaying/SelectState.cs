using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class SelectState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public SelectState(StateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player)
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
                return;
            }

            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, territory);
        }
    }
}