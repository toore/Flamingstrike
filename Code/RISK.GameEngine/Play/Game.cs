using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Play.GamePhases;

namespace RISK.GameEngine.Play
{
    public interface IGame
    {
        IPlayerInfo CurrentPlayer { get; }
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
        bool IsCurrentPlayerOccupyingTerritory(IRegion region);
    }

    public class Game : IGame
    {
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IGameStateConductor _gameStateConductor;
        private readonly Sequence<IPlayer> _players;
        private readonly IReadOnlyList<ITerritory> _initialTerritories;
        private readonly IDeck _initialDeck;
        private readonly IGameStateFsm _gameStateFsm;

        public Game(
            IGameDataFactory gameDataFactory,
            IGameStateConductor gameStateConductor,
            Sequence<IPlayer> players,
            IReadOnlyList<ITerritory> initialTerritories,
            IDeck initialDeck,
            IGameStateFsm gameStateFsm)
        {
            _gameDataFactory = gameDataFactory;
            _gameStateConductor = gameStateConductor;
            _players = players;
            _initialTerritories = initialTerritories;
            _initialDeck = initialDeck;
            _gameStateFsm = gameStateFsm;
        }

        public void Initialize()
        {
            var currentPlayer = new InGamePlayer(_players.Next());
            var players = _players
                .Select(p => new InGamePlayer(p))
                .ToList();

            var gameData = _gameDataFactory.Create(
                currentPlayer,
                players,
                _initialTerritories,
                _initialDeck);

            _gameStateConductor.InitializeFirstPlayerTurn(gameData);
        }

        public IPlayerInfo CurrentPlayer => new PlayerInfo(
            _gameStateFsm.CurrentPlayer.Player.Name,
            _gameStateFsm.CurrentPlayer.Cards.ToList());

        public ITerritory GetTerritory(IRegion region)
        {
            return _gameStateFsm.GetTerritory(region);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return _gameStateFsm.CanPlaceDraftArmies(region);
        }

        public int GetNumberOfArmiesToDraft()
        {
            return _gameStateFsm.GetNumberOfArmiesToDraft();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _gameStateFsm.PlaceDraftArmies(region, numberOfArmies);
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return _gameStateFsm.CanAttack(attackingRegion, defendingRegion);
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _gameStateFsm.Attack(attackingRegion, defendingRegion);
        }

        public bool CanSendArmiesToOccupy()
        {
            return _gameStateFsm.CanSendAdditionalArmiesToOccupy();
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return _gameStateFsm.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        }

        public void SendArmiesToOccupy(int numberOfArmies)
        {
            _gameStateFsm.SendAdditionalArmiesToOccupy(numberOfArmies);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _gameStateFsm.CanFortify(sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _gameStateFsm.Fortify(sourceRegion, destinationRegion, armies);
        }

        public void EndTurn()
        {
            _gameStateFsm.EndTurn();
        }

        public bool IsGameOver()
        {
            return false;
        }

        public bool IsCurrentPlayerOccupyingTerritory(IRegion region)
        {
            var territory = GetTerritory(region);

            var isCurrentPlayerOccupyingTerritory = territory.Player == _gameStateFsm.CurrentPlayer.Player;

            return isCurrentPlayerOccupyingTerritory;
        }
    }
}