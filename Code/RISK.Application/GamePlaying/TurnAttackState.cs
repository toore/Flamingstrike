using System;
using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class TurnAttackState : ITurnState
    {
        private readonly StateController _stateController;
        private readonly TurnStateFactory _turnStateFactory;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;
        private readonly IWorldMap _worldMap;

        public TurnAttackState(
            StateController stateController, TurnStateFactory turnStateFactory, IBattleCalculator battleCalculator, ICardFactory cardFactory, IPlayer player, IWorldMap worldMap, ITerritory selectedTerritory)
        {
            Player = player;
            _stateController = stateController;
            _turnStateFactory = turnStateFactory;
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
                _stateController.CurrentState = _turnStateFactory.CreateSelectState(Player, _worldMap);
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
            var isConnected = SelectedTerritory.Location.Borders.Contains(territoryToAttack.Location);
            var hasArmiesToAttackWith = SelectedTerritory.HasArmiesAvailableForAttack();

            var canAttack = isConnected
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
                _stateController.CurrentState = _turnStateFactory.CreateAttackState(Player, _worldMap, territory);
                _stateController.PlayerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
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