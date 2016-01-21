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
        IInGameplayPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> GetTerritories();
        ITerritory GetTerritory(ITerritoryGeography selectedTerritoryGeography);
        bool IsCurrentPlayerOccupyingTerritory(ITerritory territory);
        bool CanAttack(ITerritory attackingTerritory, ITerritory attackeeTerritory);
        void Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory);
        bool CanMoveArmiesIntoCapturedTerritory();
        void MoveArmiesIntoCapturedTerritory(int numberOfArmies);
        bool CanFortify(ITerritory sourceTerritory, ITerritory territoryToFortify);
        void Fortify(ITerritory sourceTerritory, ITerritory territoryToFortify);
        void EndTurn();
        bool IsGameOver();
    }

    public class Game : IGame
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        private bool _playerShouldReceiveCardWhenTurnEnds;
        private readonly IReadOnlyList<IInGameplayPlayer> _players;
        private bool _mustConfirmMoveOfArmiesIntoCapturedTerritory;
        private readonly IReadOnlyList<ITerritory> _territories;

        public Game(IReadOnlyList<IInGameplayPlayer> players, IReadOnlyList<ITerritory> initialTerritories, IGameRules gameRules, ICardFactory cardFactory, IBattle battle)
        {
            _players = players;
            _territories = initialTerritories;
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;

            SetStartingPlayer();
        }

        public IInGameplayPlayer CurrentPlayer { get; private set; }

        public IReadOnlyList<ITerritory> GetTerritories()
        {
            var territories = _territories.ToList();
            territories.Reverse();
            return territories;
        }

        public ITerritory GetTerritory(ITerritoryGeography selectedTerritoryGeography)
        {
            return _territories.Single(x => x.TerritoryGeography == selectedTerritoryGeography);
        }

        public bool IsCurrentPlayerOccupyingTerritory(ITerritory territory)
        {
            ThrowIfTerritoriesDoesNotContain(territory);

            var isCurrentPlayerOccupyingTerritory = territory.Player == CurrentPlayer.Player;

            return isCurrentPlayerOccupyingTerritory;
        }

        private void SetStartingPlayer()
        {
            CurrentPlayer = _players.First();
        }

        public bool CanAttack(ITerritory attackingTerritory, ITerritory attackeeTerritory)
        {
            ThrowIfTerritoriesDoesNotContain(attackingTerritory);
            ThrowIfTerritoriesDoesNotContain(attackeeTerritory);

            if (_mustConfirmMoveOfArmiesIntoCapturedTerritory
                ||
                !IsCurrentPlayerOccupyingTerritory(attackingTerritory))
            {
                return false;
            }

            var attackeeCandidates = _gameRules.GetAttackeeCandidates(attackingTerritory.TerritoryGeography, GetTerritories());
            var canAttack = attackeeCandidates.Contains(attackeeTerritory.TerritoryGeography);

            return canAttack;
        }

        public void Attack(ITerritory attackingTerritory, ITerritory attackeeTerritory)
        {
            if (!CanAttack(attackingTerritory, attackeeTerritory))
            {
                throw new InvalidOperationException();
            }

            _mustConfirmMoveOfArmiesIntoCapturedTerritory =
                IsDefenderEliminatedAfterAttack(attackingTerritory.TerritoryGeography, attackeeTerritory.TerritoryGeography);

            //if (HasPlayerOccupiedTerritory(to))
            //{
            //    _playerShouldReceiveCardWhenTurnEnds = true;
            //    return AttackResult.SucceededAndOccupying;
            //}
        }

        private bool IsDefenderEliminatedAfterAttack(ITerritoryGeography attackingTerritoryGeography, ITerritoryGeography territoryGeographyToAttack)
        {
            var attacker = GetTerritory(attackingTerritoryGeography);
            var defender = GetTerritory(territoryGeographyToAttack);

            var battleResult = _battle.Attack(attacker, defender);

            return battleResult == BattleResult.DefenderEliminated;
        }

        public bool CanMoveArmiesIntoCapturedTerritory()
        {
            return _mustConfirmMoveOfArmiesIntoCapturedTerritory;
        }

        public void MoveArmiesIntoCapturedTerritory(int numberOfArmies)
        {
            if (!CanMoveArmiesIntoCapturedTerritory())
            {
                throw new InvalidOperationException();
            }

            throw new NotImplementedException();
        }

        public bool CanFortify(ITerritory sourceTerritory, ITerritory territoryToFortify)
        {
            ThrowIfTerritoriesDoesNotContain(sourceTerritory);
            ThrowIfTerritoriesDoesNotContain(territoryToFortify);

            return false;
        }

        private void ThrowIfTerritoriesDoesNotContain(ITerritory territory)
        {
            if (!GetTerritories().Contains(territory))
            {
                throw new InvalidOperationException("Territory does not exist in game");
            }
        }

        public void Fortify(ITerritory sourceTerritory, ITerritory territoryToFortify)
        {
            if (!CanFortify(sourceTerritory, territoryToFortify))
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

        private IInGameplayPlayer GetNextPlayer()
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