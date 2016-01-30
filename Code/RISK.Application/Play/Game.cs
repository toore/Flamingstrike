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
        IReadOnlyList<ITerritory> GetTerritories();
        ITerritory GetTerritory(IRegion region);
        bool IsCurrentPlayerOccupyingTerritory(ITerritory territory);
        bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTeraritory);
        void Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
        bool MustConfirmMoveOfArmiesIntoOccupiedTerritory();
        void MoveArmiesIntoOccupiedTerritory(int numberOfArmies);
        bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory);
        void Fortify(ITerritory sourceTerritory, ITerritory destinationFortify);
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
        private bool _mustConfirmMoveOfArmiesIntoOccupiedTerritory;
        private readonly IReadOnlyList<ITerritory> _territories;

        public Game(IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> initialTerritories, IGameRules gameRules, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            _territories = initialTerritories;
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;

            SetStartingPlayer();
        }

        public IPlayer CurrentPlayer { get; private set; }

        public IReadOnlyList<ITerritory> GetTerritories()
        {
            return _territories;
        }

        public ITerritory GetTerritory(IRegion region)
        {
            return _territories.Single(x => x.Region == region);
        }

        public bool IsCurrentPlayerOccupyingTerritory(ITerritory territory)
        {
            ThrowIfTerritoriesDoesNotContain(territory);

            var isCurrentPlayerOccupyingTerritory = territory.Player == CurrentPlayer;

            return isCurrentPlayerOccupyingTerritory;
        }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            ThrowIfTerritoriesDoesNotContain(attackingTerritory);
            ThrowIfTerritoriesDoesNotContain(defendingTerritory);

            if (_mustConfirmMoveOfArmiesIntoOccupiedTerritory
                ||
                !IsCurrentPlayerOccupyingTerritory(attackingTerritory))
            {
                return false;
            }

            var candidates = _gameRules.GetAttackCandidates(attackingTerritory, _territories);
            var canAttack = candidates.Contains(defendingTerritory);

            return canAttack;
        }

        public void Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            if (!CanAttack(attackingTerritory, defendingTerritory))
            {
                throw new InvalidOperationException();
            }

            var battleResult = _battle.Attack(attackingTerritory, defendingTerritory);

            _mustConfirmMoveOfArmiesIntoOccupiedTerritory = battleResult.IsDefenderDefeated();

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}
        }

        public bool MustConfirmMoveOfArmiesIntoOccupiedTerritory()
        {
            return _mustConfirmMoveOfArmiesIntoOccupiedTerritory;
        }

        public void MoveArmiesIntoOccupiedTerritory(int numberOfArmies)
        {
            if (!MustConfirmMoveOfArmiesIntoOccupiedTerritory())
            {
                throw new InvalidOperationException();
            }

            throw new NotImplementedException();
        }

        public bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory)
        {
            ThrowIfTerritoriesDoesNotContain(sourceTerritory);
            ThrowIfTerritoriesDoesNotContain(destinationTerritory);

            return false;
        }

        private void ThrowIfTerritoriesDoesNotContain(ITerritory territory)
        {
            if (!GetTerritories().Contains(territory))
            {
                throw new InvalidOperationException("Territory does not exist in game");
            }
        }

        public void Fortify(ITerritory sourceTerritory, ITerritory destinationFortify)
        {
            if (!CanFortify(sourceTerritory, destinationFortify))
            {
                throw new InvalidOperationException();
            }

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
            var allTerritoriesAreOccupiedBySamePlayer = GetTerritories()
                .Select(x => x.Player)
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