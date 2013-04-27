using System.Linq;
using RISK.Domain.Entities;

namespace RISK.Domain.GamePlaying
{
    public class Turn : ITurn
    {
        private readonly IWorldMap _worldMap;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ICardFactory _cardFactory;
        private bool _playerShouldReceiveCardWhenTurnEnds;

        public Turn(IPlayer player, IWorldMap worldMap, IBattleCalculator battleCalculator, ICardFactory cardFactory)
        {
            Player = player;
            _worldMap = worldMap;
            _battleCalculator = battleCalculator;
            _cardFactory = cardFactory;
        }

        public ITerritory SelectedTerritory { get; private set; }
        public IPlayer Player { get; private set; }

        public bool IsTerritorySelected
        {
            get { return SelectedTerritory != null; }
        }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                Player.AddCard(_cardFactory.Create());
            }
        }

        public bool CanSelect(ILocation location)
        {
            return GetTerritory(location).AssignedPlayer == Player;
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

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.AssignedPlayer == Player;
        }

        private bool CanAttack(ITerritory territoryToAttack)
        {
            var isTerritoryOccupiedByEnemy = territoryToAttack.AssignedPlayer != Player;
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