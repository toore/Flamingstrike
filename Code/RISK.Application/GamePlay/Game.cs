using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.GamePlay.Battling;
using RISK.Application.World;

namespace RISK.Application.GamePlay
{
    public enum AttackResult
    {
        SucceededAndOccupying,
        Other
    };

    public class Game
    {
        private readonly IList<IPlayerId> _players;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;

        public Game(IList<IPlayerId> players, IWorldMap worldMap, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            PlayerId = players.First();
            WorldMap = worldMap;
            _cardFactory = cardFactory;
            _battle = battle;
        }

        public IPlayerId PlayerId { get; private set; }
        public IWorldMap WorldMap { get; }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                //PlayerId.AddCard(_cardFactory.Create());
            }

            _playerShouldReceiveCardWhenTurnEnds = false;
            PlayerId = _players.ToList().GetNextOrFirst(PlayerId);
        }

        public bool CanAttack(ITerritory from, ITerritory to)
        {
            //var isTerritoryOccupiedByEnemy = from.Occupant != PlayerId;
            //var isBordering = from.IsBordering(to);
            //var hasArmiesToAttackWith = from.HasArmiesAvailableForAttack();

            //var canAttack = isBordering
            //                &&
            //                isTerritoryOccupiedByEnemy
            //                &&
            //                hasArmiesToAttackWith;

            //return canAttack;
            return false;
        }

        public AttackResult Attack(ITerritory from, ITerritory to)
        {
            //_battle.Attack(from, to);

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}

            return AttackResult.Other;
        }

        private bool HasPlayerOccupiedTerritory(ITerritory territoryToAttack)
        {
            //return territoryToAttack.Occupant == PlayerId;
            return false;
        }

        public bool IsGameOver()
        {
            //return WorldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
            return false;
        }
    }
}