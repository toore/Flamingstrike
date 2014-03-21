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
            return GetTerritory(location).Occupant == Player;
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

        public bool CanAttack(ILocation location)
        {
            if (!IsTerritorySelected)
            {
                return false;
            }

            var territoryToAttack = GetTerritory(location);
            var isTerritoryOccupiedByEnemy = territoryToAttack.Occupant != Player;
            var isConnected = SelectedTerritory.Location.Connections.Contains(territoryToAttack.Location);
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

            var territory = GetTerritory(location);
            Attack(territory);
        }

        private void Attack(ITerritory territory)
        {
            _battleCalculator.Attack(SelectedTerritory, territory);

            if (HasPlayerOccupiedTerritory(territory))
            {
                SelectedTerritory = territory;
                _playerShouldReceiveCardWhenTurnEnds = true;
            }
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
        }

        private ITerritory GetTerritory(ILocation location)
        {
            return _worldMap.GetTerritory(location);
        }
    }
}