using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.World;

namespace RISK.Application.Play
{
    // GameStateFactory?

    public interface IGame
    {
        /* Test:
        Attack of bordering territory: occupied by player, occupied by other player
        Attack of remote territory: occupied by player, occupied by other player
        */
        //IGameState Start(); // Same as EndTurn technically, but semantically different
        void Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory);
        //IGameState Fortify(ITerritory fromTerritory, ITerritory toTerritory);
        void EndTurn();
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        bool IsGameOver();
        bool CanAttack(ITerritory attackingTerritory, ITerritory attackedTerritory);
        bool IsCurrentPlayerOccupyingTerritory(ITerritory territory);
    }

    public class Game : IGame
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly List<IPlayer> _players;

        public Game(IEnumerable<IPlayer> players, IReadOnlyList<IGameboardTerritory> gameboardTerritories, IGameRules gameRules, ICardFactory cardFactory, IBattle battle)
        {
            _players = players.ToList();
            GameboardTerritories = gameboardTerritories;
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;

            SetStartingPlayer();
        }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public void Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory)
        {
            // TODO: throw if attacker is not current player
            // TODO: throw if attacker is same as attackee

            //_battle.Attack(from, to);

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}
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

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }

        public bool IsGameOver()
        {
            var allTerritoriesAreOccupiedBySamePlayer = GameboardTerritories.Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }

        public bool CanAttack(ITerritory attackingTerritory, ITerritory attackedTerritory)
        {
            var attackeeCandidates = _gameRules.GetAttackeeCandidates(attackingTerritory, GameboardTerritories);
            var canAttack = attackeeCandidates.Contains(attackedTerritory);

            return canAttack;
        }

        public bool IsCurrentPlayerOccupyingTerritory(ITerritory territory)
        {
            var gameboardTerritory = GameboardTerritories.GetFromTerritory(territory);
            var isCurrentPlayerOccupyingTerritory = gameboardTerritory.Player == CurrentPlayer;

            //return isCurrentPlayerOccupyingTerritory;
            throw new NotImplementedException();
        }

        //private IGameState CreateGameState()
        //{
        //    return new GameState(_currentPlayer, _players, _gameboardTerritories, _gameboardRules);
        //}
    }
}