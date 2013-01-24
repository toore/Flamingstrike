using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class Turn : ITurn 
    {
        private readonly IPlayer _player;
        private readonly IWorldMap _worldMap;
        private readonly IBattleCalculator _battleCalculator;
        private ITerritory _selectedTerritory;

        public Turn(IPlayer player, IWorldMap worldMap, IBattleCalculator battleCalculator)
        {
            _player = player;
            _worldMap = worldMap;
            _battleCalculator = battleCalculator;
        }

        public void SelectTerritory(ITerritoryLocation territoryLocation)
        {
            var territory = GetTerritory(territoryLocation);
            if (territory.Owner == _player)
            {
                _selectedTerritory = territory;
            }
        }

        private ITerritory GetTerritory(ITerritoryLocation territoryLocation)
        {
            return _worldMap.GetTerritory(territoryLocation);
        }

        public void AttackTerritory(ITerritoryLocation territoryLocation)
        {
            var isConnected = _selectedTerritory.TerritoryLocation.ConnectedTerritories.Contains(territoryLocation);
            var territoryToAttack = GetTerritory(territoryLocation);
            var isTerritoryOccupiedByEnemy = territoryToAttack.Owner != _player;
            if (isConnected && isTerritoryOccupiedByEnemy)
            {
                _battleCalculator.Attack(_selectedTerritory, territoryToAttack);
            }
        }

        public bool PlayerShouldReceiveCardWhenTurnEnds()
        {
            throw new System.NotImplementedException();
        }
    }
}