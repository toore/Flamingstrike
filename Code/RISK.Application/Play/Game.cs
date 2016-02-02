using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.Play.GamePhases;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGame
    {
        IPlayer CurrentPlayer { get; }
        ITerritory GetTerritory(IRegion region);
        int GetNumberOfArmiesToDraft();
        bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory);
        void Attack(ITerritory attackingTerritory, ITerritory defendingTerritory);
        bool MustSendInArmiesToOccupyTerritory();
        void SendInArmiesToOccupyTerritory(int numberOfArmies);
        bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory);
        void Fortify(ITerritory sourceTerritory, ITerritory destinationFortify);
        void EndTurn();
        bool IsGameOver();
    }

    public class Game : IGame
    {
        private readonly IGameStateFactory _gameStateFactory;
        private IGameState _gameState;

        public Game(IGameStateFactory gameStateFactory, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> initialTerritories)
        {
            _gameStateFactory = gameStateFactory;

            var gameData = new GameData(players.First(), players, initialTerritories.ToList());

            Initialize(gameData);
        }

        public IPlayer CurrentPlayer => _gameState.CurrentPlayer;

        private void Initialize(GameData gameData)
        {
            _gameState = _gameStateFactory.CreateDraftArmiesGameState(gameData);
        }

        public ITerritory GetTerritory(IRegion region)
        {
            return _gameState.Territories.Single(x => x.Region == region);
        }

        public int GetNumberOfArmiesToDraft()
        {
            throw new NotImplementedException();
        }

        public bool CanAttack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return _gameState.CanAttack(attackingTerritory, defendingTerritory);
        }

        public void Attack(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            _gameState = _gameState.Attack(attackingTerritory, defendingTerritory);
        }

        public bool MustSendInArmiesToOccupyTerritory()
        {
            //return _mustSendInArmiesToOccupyTerritory;
            return _gameState.MustSendInArmiesToOccupyTerritory();
        }

        public void SendInArmiesToOccupyTerritory(int numberOfArmies)
        {
            //if (!_mustSendInArmiesToOccupyTerritory)
            //{
            //    throw new InvalidOperationException();
            //}

            throw new NotImplementedException();
        }

        public bool CanFortify(ITerritory sourceTerritory, ITerritory destinationTerritory)
        {
            return _gameState.CanFortify(sourceTerritory, destinationTerritory);
            //ThrowIfTerritoriesDoesNotContain(sourceTerritory);
            //ThrowIfTerritoriesDoesNotContain(destinationTerritory);

            //return false;
        }

        public void Fortify(ITerritory sourceTerritory, ITerritory destinationFortify)
        {
            _gameState = _gameState.Fortify(sourceTerritory, destinationFortify);
            //if (!CanFortify(sourceTerritory, destinationFortify))
            //{
            //    throw new InvalidOperationException();
            //}

            //throw new NotImplementedException();
        }

        public void EndTurn()
        {
            _gameState = _gameState.EndTurn();
        }

        public bool IsGameOver()
        {
            return _gameState.IsGameOver();
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

    public class GameData
    {
        public GameData(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IList<ITerritory> territories)
        {
            CurrentPlayer = currentPlayer;
            Players = players;
            Territories = territories;
        }

        public IPlayer CurrentPlayer { get; }

        public IReadOnlyList<IPlayer> Players { get; }

        public IList<ITerritory> Territories { get; }
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