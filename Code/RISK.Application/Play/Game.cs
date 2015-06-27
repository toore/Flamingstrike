using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.Setup;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public enum AttackResult
    {
        SucceededAndOccupying,
        Other
    };

    public interface IGame
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        IEnumerable<ITerritory> GetAttackeeCandidates(ITerritory attackingTerritory);
        void EndTurn();
        bool IsGameOver();

        /* Test:
        Attack of bordering territory: occupied by player, occupied by other player
        Attack of remote territory: occupied by player, occupied by other player
        */
        AttackResult Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory);
        bool IsCurrentPlayerOccupyingTerritory(ITerritory territory);
    }

    public class Game : IGame
    {
        private readonly IGameboardRules _gameboardRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly List<IPlayer> _players;
        private readonly List<GameboardTerritory> _gameboardTerritories;

        public Game(IGameSetup gameSetup, IGameboardRules gameboardRules, ICardFactory cardFactory, IBattle battle, ITerritoryConverter territoryConverter)
        {
            _gameboardRules = gameboardRules;
            _cardFactory = cardFactory;
            _battle = battle;
            _players = gameSetup.Players.ToList();
            CurrentPlayer = gameSetup.Players.First();

            _gameboardTerritories = territoryConverter.Convert(gameSetup.GameboardTerritories);
        }

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyList<IGameboardTerritory> GameboardTerritories => _gameboardTerritories;

        public IEnumerable<ITerritory> GetAttackeeCandidates(ITerritory attackingTerritory)
        {
            return _gameboardRules.GetAttackeeCandidates(_gameboardTerritories, attackingTerritory);
        }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                //PlayerId.AddCard(_cardFactory.Create());
            }

            _playerShouldReceiveCardWhenTurnEnds = false;
            CurrentPlayer = _players.GetNextOrFirst(CurrentPlayer);
        }

        public AttackResult Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory)
        {
            //_battle.Attack(from, to);

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}

            return AttackResult.Other;
        }

        public bool IsCurrentPlayerOccupyingTerritory(ITerritory territory)
        {
            throw new System.NotImplementedException();
        }

        public bool IsGameOver()
        {
            //return WorldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
            return false;
        }
    }
}