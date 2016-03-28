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
        bool CanPlaceDraftArmies(IRegion region);
        int GetNumberOfArmiesToDraft();
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendArmiesToOccupy();
        int GetNumberOfArmiesThatCanBeSentToOccupy();
        void SendArmiesToOccupy(int numberOfArmies);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        void EndTurn();
        bool IsGameOver();
    }

    public class Game : IGame
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private IGameState _gameState;

        public Game(IGameStateFactory gameStateFactory, IArmyDraftCalculator armyDraftCalculator, Sequence<IPlayer> players, IReadOnlyList<ITerritory> initialTerritories)
        {
            _gameStateFactory = gameStateFactory;
            _armyDraftCalculator = armyDraftCalculator;

            var gameData = new GameData(players.Next(), players.ToList(), initialTerritories.ToList());

            Initialize(gameData);
        }

        public IPlayer CurrentPlayer => _gameState.CurrentPlayer;

        private void Initialize(GameData gameData)
        {
            var numberOfArmiesToDraft = _armyDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);
            _gameState = _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesToDraft);
        }

        public ITerritory GetTerritory(IRegion region)
        {
            return _gameState.GetTerritory(region);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return _gameState.CanPlaceDraftArmies(region);
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

        public bool CanSendArmiesToOccupy()
        {
            return _gameState.CanSendArmiesToOccupy();
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return _gameState.GetNumberOfArmiesThatCanBeSentToOccupy();
        }

        public void SendArmiesToOccupy(int numberOfArmies)
        {
            _gameState = _gameState.SendArmiesToOccupy(numberOfArmies);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _gameState.CanFortify(sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _gameState = _gameState.Fortify(sourceRegion, destinationRegion, 1);
        }

        public void EndTurn()
        {
            _gameState = _gameState.EndTurn();
        }

        public bool IsGameOver()
        {
            return false;
        }
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