using System;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnSelectState : ITurnState
    {
        private readonly StateController _stateController;
        private readonly TurnStateFactory _turnStateFactory;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;

        public TurnSelectState(StateController stateController, TurnStateFactory turnStateFactory, ICardFactory cardFactory, IPlayer player, IWorldMap worldMap)
        {
            Player = player;
            _stateController = stateController;
            _turnStateFactory = turnStateFactory;
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
            _stateController.CurrentState = _turnStateFactory.CreateAttackState(Player, _worldMap, territoryToSelect);
        }

        public bool CanAttack(ILocation location)
        {
            return false;
        }

        public void Attack(ILocation location)
        {
            throw new NotSupportedException();
        }

        public bool IsFortificationAllowedInTurn()
        {
            return true;
        }

        public bool CanFortify(ILocation location)
        {
            throw new NotSupportedException();
        }

        public void Fortify(ILocation location, int armies)
        {
            throw new NotImplementedException();
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