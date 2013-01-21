using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class Turn : ITurn 
    {
        private readonly IPlayer _player;
        private readonly IWorldMap _worldMap;
        private readonly IBattleEvaluater _battleEvaluater;
        private IArea _selectedArea;

        public Turn(IPlayer player, IWorldMap worldMap, IBattleEvaluater battleEvaluater)
        {
            _player = player;
            _worldMap = worldMap;
            _battleEvaluater = battleEvaluater;
        }

        public void SelectArea(IAreaDefinition areaDefinition)
        {
            var area = GetArea(areaDefinition);
            if (area.Owner == _player)
            {
                _selectedArea = area;
            }
        }

        private IArea GetArea(IAreaDefinition areaDefinition)
        {
            return _worldMap.GetArea(areaDefinition);
        }

        public void AttackArea(IAreaDefinition areaDefinition)
        {
            var isAreaConnected = _selectedArea.AreaDefinition.ConnectedAreas.Contains(areaDefinition);
            var areaToAttack = GetArea(areaDefinition);
            var isAreaOccupiedByEnemy = areaToAttack.Owner != _player;
            if (isAreaConnected && isAreaOccupiedByEnemy)
            {
                //_battleEvaluater.Attack(_selectedArea, areaToAttack);
                //area
            }
        }

        public bool PlayerShouldReceiveCardWhenTurnEnds()
        {
            throw new System.NotImplementedException();
        }
    }
}