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
        ITerritory GetTerritory(IRegion region);
        bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory);
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
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly IReadOnlyList<IPlayer> _players;
        private bool _mustConfirmMoveOfArmiesIntoOccupiedTerritory;
        private readonly IReadOnlyList<ITerritory> _territories;

        public Game(IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> initialTerritories, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            _territories = initialTerritories;
            _cardFactory = cardFactory;
            _battle = battle;

            SetStartingPlayer();
        }

        public IPlayer CurrentPlayer { get; private set; }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public ITerritory GetTerritory(IRegion region)
        {
            return _territories.Single(x => x.Region == region);
        }

        public bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            ThrowIfTerritoriesDoesNotContain(attackingTerritory);
            ThrowIfTerritoriesDoesNotContain(defendingTerritory);
            if (_mustConfirmMoveOfArmiesIntoOccupiedTerritory
                ||
                !IsCurrentPlayerAttacking(attackingTerritory))
            {
                return false;
            }

            var canAttack = HasBorder(attackingTerritory, defendingTerritory)
                            &&
                            IsAttackerAndDefenderDifferentPlayers(attackingTerritory, defendingTerritory)
                            &&
                            HasAttackerEnoughArmiesToPerformAttack(attackingTerritory);

            return canAttack;
        }

        private bool IsCurrentPlayerAttacking(ITerritory attackingTerritory)
        {
            return CurrentPlayer == attackingTerritory.Player;
        }

        private static bool HasBorder(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Region.HasBorder(defendingTerritory.Region);
        }

        private static bool HasAttackerEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesAvailableForAttack() > 0;
        }

        private static bool IsAttackerAndDefenderDifferentPlayers(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Player != defendingTerritory.Player;
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
            if (!_mustConfirmMoveOfArmiesIntoOccupiedTerritory)
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
            if (!_territories.Contains(territory))
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
            var allTerritoriesAreOccupiedBySamePlayer = _territories
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

    public static class GameExtensions
    {
        public static bool IsCurrentPlayerOccupyingTerritory(this IGame game, IRegion region)
        {
            var territory = game.GetTerritory(region);

            var isCurrentPlayerOccupyingTerritory = territory.Player == game.CurrentPlayer;

            return isCurrentPlayerOccupyingTerritory;
        }
    }
}