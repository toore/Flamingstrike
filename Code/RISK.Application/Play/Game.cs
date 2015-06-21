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
        IGameState GameState { get; }
        void EndTurn();
        bool IsGameOver();
        AttackResult Attack(ITerritory from, ITerritory to);
    }

    public class Game : IGame
    {
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly List<IPlayer> _players;
        private IPlayer _currentPlayer;
        private readonly List<GameboardTerritory> _gameboardTerritories;

        public Game(ICardFactory cardFactory, IBattle battle, IGameSetup gameSetup)
        {
            _cardFactory = cardFactory;
            _battle = battle;
            _players = gameSetup.Players.ToList();
            _currentPlayer = gameSetup.Players.First();
            _gameboardTerritories = gameSetup.GameboardTerritories
                .Select(Convert)
                .ToList();
        }

        public IGameState GameState => CreateGameState();

        private static GameboardTerritory Convert(IGameboardSetupTerritory gameboardSetupTerritory)
        {
            return new GameboardTerritory(
                gameboardSetupTerritory.Territory,
                gameboardSetupTerritory.Player,
                gameboardSetupTerritory.Armies);
        }

        private IGameState CreateGameState()
        {
            return new GameState
            {
                CurrentPlayer = _currentPlayer,
                Territories = _gameboardTerritories
            };
        }

        public void EndTurn()
        {
            if (_playerShouldReceiveCardWhenTurnEnds)
            {
                //PlayerId.AddCard(_cardFactory.Create());
            }

            _playerShouldReceiveCardWhenTurnEnds = false;
            _currentPlayer = _players.GetNextOrFirst(_currentPlayer);
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