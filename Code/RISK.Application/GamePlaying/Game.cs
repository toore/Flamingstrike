using System.Linq;
using RISK.Application.Extensions;

namespace RISK.Application.GamePlaying
{
    public enum AttackResult
    {
        SucceededAndOccupying,
        Other
    };

    public class Game
    {
        private readonly IOrderedEnumerable<IPlayer> _players;
        private readonly ICardFactory _cardFactory;
        private readonly IBattleCalculator _battleCalculator;

        private bool _playerShouldReceiveCardWhenTurnEnds;

        public Game(IOrderedEnumerable<IPlayer> players, IWorldMap worldMap, ICardFactory cardFactory, IBattleCalculator battleCalculator)
        {
            _players = players;
            Player = players.First();
            WorldMap = worldMap;
            _cardFactory = cardFactory;
            _battleCalculator = battleCalculator;
        }

        public IPlayer Player { get; private set; }
        public IWorldMap WorldMap { get; private set; }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                Player.AddCard(_cardFactory.Create());
            }

            _playerShouldReceiveCardWhenTurnEnds = false;
            Player = _players.ToList().GetNextOrFirst(Player);
        }

        public bool CanAttack(ITerritory from, ITerritory to)
        {
            var isTerritoryOccupiedByEnemy = from.Occupant != Player;
            var isBordering = from.IsBordering(to);
            var hasArmiesToAttackWith = from.HasArmiesAvailableForAttack();

            var canAttack = isBordering
                            &&
                            isTerritoryOccupiedByEnemy
                            &&
                            hasArmiesToAttackWith;

            return canAttack;
        }

        public AttackResult Attack(ITerritory from, ITerritory to)
        {
            _battleCalculator.Attack(from, to);

            if (HasPlayerOccupiedTerritory(to))
            {
                _playerShouldReceiveCardWhenTurnEnds = true;
                return AttackResult.SucceededAndOccupying;
            }

            return AttackResult.Other;
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            return territoryToAttack.Occupant == Player;
        }

        public bool IsGameOver()
        {
            return WorldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
        }
    }
}