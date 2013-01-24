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

        public void SelectArea(ITerritoryLocation territoryLocation)
        {
            var area = GetArea(territoryLocation);
            if (area.Owner == _player)
            {
                _selectedTerritory = area;
            }
        }

        private ITerritory GetArea(ITerritoryLocation territoryLocation)
        {
            return _worldMap.GetArea(territoryLocation);
        }

        public void AttackArea(ITerritoryLocation territoryLocation)
        {
            var isAreaConnected = _selectedTerritory.TerritoryLocation.ConnectedTerritories.Contains(territoryLocation);
            var areaToAttack = GetArea(territoryLocation);
            var isAreaOccupiedByEnemy = areaToAttack.Owner != _player;
            if (isAreaConnected && isAreaOccupiedByEnemy)
            {
                _battleCalculator.Attack(_selectedTerritory, areaToAttack);
            }
        }

        public bool PlayerShouldReceiveCardWhenTurnEnds()
        {
            throw new System.NotImplementedException();
        }
    }
}