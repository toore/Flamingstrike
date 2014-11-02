using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class SelectState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMap _worldMap;

        public SelectState(StateController stateController, IInteractionStateFactory interactionStateFactory, IPlayer player, IWorldMap worldMap)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            _worldMap = worldMap;
        }

        public ITerritory SelectedTerritory
        {
            get { return null; }
        }

        public IPlayer Player { get; private set; }

        public bool CanClick(ILocation location)
        {
            return _worldMap.GetTerritory(location).Occupant == Player;
        }

        public void OnClick(ILocation location)
        {
            if (!CanClick(location))
            {
                return;
            }

            var territoryToSelect = _worldMap.GetTerritory(location);
            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(_stateController, Player, _worldMap, territoryToSelect);
        }
    }
}