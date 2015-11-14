using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play.Attacking;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGame
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        bool IsCurrentPlayerOccupyingTerritory(ITerritoryId territoryId);
        bool CanAttack(ITerritoryId attackingTerritoryId, ITerritoryId territoryIdToAttack);
        void Attack(ITerritoryId attackingTerritoryId, ITerritoryId attackeeTerritoryId);
        bool CanFortify(ITerritoryId sourceIdTerritory, ITerritoryId territoryIdToFortify);
        void Fortify(ITerritoryId selectedTerritoryId, ITerritoryId territoryIdToFortify);
        void EndTurn();
        bool IsGameOver();
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

        public bool IsCurrentPlayerOccupyingTerritory(ITerritoryId territoryId)
        {
            var territory = Territories.Get(territoryId);
            var isCurrentPlayerOccupyingTerritory = territory.PlayerId == CurrentPlayer.PlayerId;

            return isCurrentPlayerOccupyingTerritory;
        }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public bool CanAttack(ITerritoryId attackingTerritoryId, ITerritoryId territoryIdToAttack)
        {
            if (!IsCurrentPlayerOccupyingTerritory(attackingTerritoryId))
            {
                return false;
            }

            var attackeeCandidates = _gameRules.GetAttackeeCandidates(attackingTerritoryId, Territories);
            var canAttack = attackeeCandidates.Contains(territoryIdToAttack);

            return canAttack;
        }

        public void Attack(ITerritoryId attackingTerritoryId, ITerritoryId territoryIdToAttack)
        {
            if (!CanAttack(attackingTerritoryId, territoryIdToAttack))
            {
                throw new InvalidOperationException();
            }

            // TODO: throw if attacker is not current player
            // TODO: throw if attacker is same as attackee

            //_battle.Attack(from, to);

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}
        }

        public bool CanFortify(ITerritoryId sourceIdTerritory, ITerritoryId territoryIdToFortify)
        {
            throw new NotImplementedException();
        }

        public void Fortify(ITerritoryId selectedTerritoryId, ITerritoryId territoryIdToFortify)
        {
            throw new NotImplementedException();
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
            var allTerritoriesAreOccupiedBySamePlayer = Territories
                .Select(x => x.PlayerId)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }

        //public FortifyMoveState(ITerritory selectedTerritory)
        //{
        //    SelectedTerritory = selectedTerritory;
        //}

        //public ITerritory SelectedTerritory { get; }

        //public bool CanClick(ITerritory territory)
        //{
        //    return true;

        //    //return
        //    //    SelectedTerritory.IsBordering(territory)
        //    //        &&
        //    //    territory.Occupant == Player;
        //}

        //public void OnClick(ITerritory territory)
        //{
        //    throw new NotImplementedException();
        //}

        ////public bool CanFortify(ILocation location)
        ////{
        ////    return SelectedTerritory.Location.IsBordering(location) 
        ////        && 
        ////        _worldMap.GetTerritory(location).Occupant == Player;
        ////}

        ////public void Fortify(ILocation location, int armies)
        ////{
        ////    _worldMap.GetTerritory(location).Armies += armies;
        ////    SelectedTerritory.Armies -= armies;

        ////    _stateController.CurrentState = _interactionStateFactory.CreateFortifiedState(Player, _worldMap);
        ////}
    }
}