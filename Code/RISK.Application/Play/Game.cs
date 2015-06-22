using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Battling;
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
        IReadOnlyList<IGameboardTerritory> Territories { get; }
        IEnumerable<ITerritory> GetAttackeeCandidates(ITerritory attackingTerritory);
        void EndTurn();
        bool IsGameOver();
        AttackResult Attack(ITerritory from, ITerritory to);
    }

    public class Game : IGame
    {
        private readonly IGameboardRules _gameboardRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly List<IPlayer> _players;
        private readonly List<GameboardTerritory> _territories;

        public Game(IGameSetup gameSetup, IGameboardRules gameboardRules, ICardFactory cardFactory, IBattle battle, ITerritoryConverter territoryConverter)
        {
            _gameboardRules = gameboardRules;
            _cardFactory = cardFactory;
            _battle = battle;
            _players = gameSetup.Players.ToList();
            CurrentPlayer = gameSetup.Players.First();

            _territories = territoryConverter.Convert(gameSetup.GameboardTerritories);
        }

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyList<IGameboardTerritory> Territories => _territories;



        public IEnumerable<ITerritory> GetAttackeeCandidates(ITerritory attackingTerritory)
        {
            return _gameboardRules.GetAttackeeCandidates(_territories, attackingTerritory);
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

        public bool IsGameOver()
        {
            //return WorldMap.GetAllPlayersOccupyingTerritories().Count() == 1;
            return false;
        }
    }


}