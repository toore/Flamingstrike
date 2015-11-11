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
        void Attack(ITerritoryId attackingTerritoryId, ITerritoryId attackeeTerritoryId);
        //IGameState Fortify(ITerritory fromTerritory, ITerritory toTerritory);
        void EndTurn();
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        bool IsGameOver();
        bool IsCurrentPlayerOccupyingTerritory(ITerritoryId territoryId);
        bool CanAttack(ITerritoryId attackingTerritoryId, ITerritoryId territoryIdToAttack);
        bool CanFortify(ITerritoryId sourceTerritory, ITerritoryId territoryIdToFortify);
    }

    public class Game : IGame
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly IReadOnlyList<IPlayer> _players;

        public Game(IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories, IGameRules gameRules, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            Territories = territories;
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;

            SetStartingPlayer();
        }

        public IPlayer CurrentPlayer { get; private set; }
        public IReadOnlyList<ITerritory> Territories { get; }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public void Attack(ITerritoryId attackingTerritoryId, ITerritoryId attackeeTerritoryId)
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
            CurrentPlayer = GetNextPlayer();
        }

        private IPlayer GetNextPlayer()
        {
            return _players.ToList().GetNextOrFirst(CurrentPlayer);
        }

        public bool IsGameOver()
        {
            var allTerritoriesAreOccupiedBySamePlayer = Territories.Select(x => x.PlayerId)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }

        public bool CanAttack(ITerritoryId attackingTerritoryId, ITerritoryId territoryIdToAttack)
        {
            var attackeeCandidates = _gameRules.GetAttackeeCandidates(attackingTerritoryId, Territories);
            var canAttack = attackeeCandidates.Contains(territoryIdToAttack);

            return canAttack;
        }

        public bool CanFortify(ITerritoryId sourceTerritory, ITerritoryId territoryIdToFortify)
        {
            throw new NotImplementedException();
        }

        public bool IsCurrentPlayerOccupyingTerritory(ITerritoryId territoryId)
        {
            var gameboardTerritory = Territories.Get(territoryId);
            var isCurrentPlayerOccupyingTerritory = gameboardTerritory.PlayerId == CurrentPlayer.PlayerId;

            //return isCurrentPlayerOccupyingTerritory;
            throw new NotImplementedException();
        }

        //private IGameState CreateGameState()
        //{
        //    return new GameState(_currentPlayer, _players, _gameboardTerritories, _gameboardRules);
        //}
    }
}