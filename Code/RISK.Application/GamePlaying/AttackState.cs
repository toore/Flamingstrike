using System;
using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class AttackState : IInteractionState
    {
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;

        public AttackState(
            StateController stateController, 
            IInteractionStateFactory interactionStateFactory, 
            IBattleCalculator battleCalculator, 
            ICardFactory cardFactory, 
            IPlayer player, 
            IWorldMap worldMap, 
            ITerritory selectedTerritory)
        {
            Player = player;
            _stateController = stateController;
            _interactionStateFactory = interactionStateFactory;
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
            _worldMap = worldMap;
            SelectedTerritory = selectedTerritory;
        }

        public IPlayer Player { get; private set; }
        public ITerritory SelectedTerritory { get; private set; }

        public bool IsTerritorySelected
        {
            get { return true; }
        }

        public bool CanSelect(ILocation location)
        {
            return location == SelectedTerritory.Location;
        }

        public void Select(ILocation location)
        {
            if (CanSelect(location))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateSelectState(Player, _worldMap);
            }
        }

        public bool CanAttack(ILocation location)
        {
            if (!IsTerritorySelected)
            {
                return false;
            }

            var territoryToAttack = _worldMap.GetTerritory(location);
            var isTerritoryOccupiedByEnemy = territoryToAttack.Occupant != Player;
            var isBordering = SelectedTerritory.Location.IsBordering(territoryToAttack.Location);
            var hasArmiesToAttackWith = SelectedTerritory.HasArmiesAvailableForAttack();

            var canAttack = isBordering
                            &&
                            isTerritoryOccupiedByEnemy
                            &&
                            hasArmiesToAttackWith;

            return canAttack;
        }

        public void Attack(ILocation location)
        {
            var canAttack = CanAttack(location);

            if (!canAttack)
            {
                return;
            }

            var territory = _worldMap.GetTerritory(location);
            Attack(territory);
        }

        private void Attack(ITerritory territory)
        {
            _battleCalculator.Attack(SelectedTerritory, territory);

            if (HasPlayerOccupiedTerritory(territory))
            {
                _stateController.CurrentState = _interactionStateFactory.CreateAttackState(Player, _worldMap, territory);
                _stateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
        }

        public bool CanFortify(ILocation location)
        {
            return SelectedTerritory.Location.IsBordering(location) 
                && 
                _worldMap.GetTerritory(location).Occupant == Player;
        }

        public void Fortify(ILocation location, int armies)
        {
            _worldMap.GetTerritory(location).Armies += armies;
            SelectedTerritory.Armies -= armies;

            _stateController.CurrentState = _interactionStateFactory.CreateFortifiedState(Player, _worldMap);
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