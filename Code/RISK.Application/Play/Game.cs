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
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendInArmiesToOccupy();
        void SendInArmiesToOccupy(int numberOfArmies);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion);
        void EndTurn();
        bool IsGameOver();
    }

    public class Game : IGame
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly INewArmiesDraftCalculator _newArmiesDraftCalculator;
        private IGameState _gameState;

        public Game(IGameStateFactory gameStateFactory, INewArmiesDraftCalculator newArmiesDraftCalculator, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> initialTerritories)
        {
            _gameStateFactory = gameStateFactory;
            _newArmiesDraftCalculator = newArmiesDraftCalculator;

            var gameData = new GameData(players.First(), players, initialTerritories.ToList());

            Initialize(gameData);
        }

        public IPlayer CurrentPlayer => _gameState.CurrentPlayer;

        private void Initialize(GameData gameData)
        {
            var numberOfArmiesToDraft = _newArmiesDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);
            _gameState = _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesToDraft);
        }

        public ITerritory GetTerritory(IRegion region)
        {
            return _gameState.GetTerritory(region);
        }

        public int GetNumberOfArmiesToDraft()
        {
            return _gameState.GetNumberOfArmiesToDraft();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _gameState = _gameState.PlaceDraftArmies(region, numberOfArmies);
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return _gameState.CanAttack(attackingRegion, defendingRegion);
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _gameState = _gameState.Attack(attackingRegion, defendingRegion);
        }

        public bool CanSendInArmiesToOccupy()
        {
            //return _mustSendInArmiesToOccupyTerritory;
            return _gameState.CanSendInArmiesToOccupy();
        }

        public void SendInArmiesToOccupy(int numberOfArmies)
        {
            _gameState = _gameState.SendInArmiesToOccupy(numberOfArmies);
            //if (!_mustSendInArmiesToOccupyTerritory)
            //{
            //    throw new InvalidOperationException();
            //}

            throw new NotImplementedException();
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _gameState.CanFortify(sourceRegion, destinationRegion);
            //ThrowIfTerritoriesDoesNotContain(sourceTerritory);
            //ThrowIfTerritoriesDoesNotContain(destinationTerritory);

            //return false;
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            _gameState = _gameState.Fortify(sourceRegion, destinationRegion);
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
        public GameData(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories)
        {
            CurrentPlayer = currentPlayer;
            Players = players;
            Territories = territories;
        }

        public IPlayer CurrentPlayer { get; }

        public IReadOnlyList<IPlayer> Players { get; }

        public IReadOnlyList<ITerritory> Territories { get; }
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