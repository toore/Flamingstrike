using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class Turn : ITurn
    {
        private readonly IPlayer _player;
        private readonly IWorldMap _worldMap;
        private readonly IBattleCalculator _battleCalculator;
        private bool _playerShouldReceiveCardWhenTurnEnds;

        public Turn(IPlayer player, IWorldMap worldMap, IBattleCalculator battleCalculator)
        {
            _player = player;
            _worldMap = worldMap;
            _battleCalculator = battleCalculator;
        }

        public ITerritory SelectedTerritory { get; private set; }

        public bool IsTerritorySelected
        {
            get { return SelectedTerritory != null; }
        }

        public bool CanSelect(ILocation location)
        {
            return GetTerritory(location).Owner == _player;
        }

        public void Select(ILocation location)
        {
            if (!CanSelect(location))
            {
                return;
            }

            var territoryToSelect = GetTerritory(location);
            if (territoryToSelect == SelectedTerritory)
            {
                SelectedTerritory = null;
            }
            else
            {
                SelectedTerritory = territoryToSelect;
            }
        }

        public void Attack(ILocation location)
        {
            if (IsTerritorySelected)
            {
                AttackFromSelected(location);
            }
        }

        private void AttackFromSelected(ILocation location)
        {
            var territoryToAttack = GetTerritory(location);
            var canAttack = CanAttack(territoryToAttack);

            if (canAttack)
            {
                Attack(territoryToAttack);
            }
        }

        private void Attack(ITerritory territory)
        {
            _battleCalculator.Attack(SelectedTerritory, territory);

            if (HasPlayerOccupiedTerritory(territory))
            {
                _playerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        public bool PlayerShouldReceiveCardWhenTurnEnds()
        {
            return _playerShouldReceiveCardWhenTurnEnds;
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Owner == _player;
        }

        private bool CanAttack(ITerritory territoryToAttack)
        {
            var isTerritoryOccupiedByEnemy = territoryToAttack.Owner != _player;
            var isConnected = SelectedTerritory.Location.Connections.Contains(territoryToAttack.Location);
            var canAttack = isConnected && isTerritoryOccupiedByEnemy;

            return canAttack;
        }

        private ITerritory GetTerritory(ILocation location)
        {
            return _worldMap.GetTerritory(location);
        }
    }
}