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
        private readonly IList<IPlayer> _players;
        private readonly IGameboard _gameboard;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;

        public Game(IList<IPlayer> players, IWorldMap worldMap, IGameboard gameboard, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            WorldMap = worldMap;
            _gameboard = gameboard;
            _cardFactory = cardFactory;
            _battle = battle;

            Player = _players.First();
        }

        public IPlayer Player { get; private set; }
        public IWorldMap WorldMap { get; }
        public IGameboard Gameboard { get; private set; }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                //PlayerId.AddCard(_cardFactory.Create());
            }

            _playerShouldReceiveCardWhenTurnEnds = false;
            Player = _players.ToList().GetNextOrFirst(Player);
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