using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class SelectState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;

        public SelectState(StateController stateController, IInteractionStateFactory interactionStateFactory, ICardFactory cardFactory, IPlayer player, IWorldMap worldMap)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            _cardFactory = cardFactory;
            _worldMap = worldMap;
        }

        public ITerritory SelectedTerritory
        {
            get { return null; }
        }

        public IPlayer Player { get; private set; }

        public bool IsTerritorySelected
        {
            get { return SelectedTerritory != null; }
        }

        public bool CanSelect(ILocation location)
        {
            return _worldMap.GetTerritory(location).Occupant == Player;
        }

        public void Select(ILocation location)
        {
            if (!CanSelect(location))
            {
                return;
            }

            var territoryToSelect = _worldMap.GetTerritory(location);
            _stateController.CurrentState = _interactionStateFactory.CreateAttackState(Player, _worldMap, territoryToSelect);
        }

        public bool CanAttack(ILocation location)
        {
            return false;
        }

        public void Attack(ILocation location)
        {
            throw new NotSupportedException();
        }

        public bool CanFortify(ILocation location)
        {
            return false;
        }

        public void Fortify(ILocation location, int armies)
        {
            throw new NotSupportedException();
        }

        public void EndTurn()
        {
            if (_stateController.PlayerShouldReceiveCardWhenTurnEnds)
            {
                Player.AddCard(_cardFactory.Create());
            }
        }
    }
}