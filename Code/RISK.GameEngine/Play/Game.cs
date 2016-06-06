using RISK.Core;
using RISK.GameEngine.Play.GamePhases;

namespace RISK.GameEngine.Play
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
        bool IsCurrentPlayerOccupyingTerritory(IRegion region);
    }

    public class Game : IGame
    {
        private readonly IGameStateFsm _gameStateFsm;

        public Game(IGameStateFsm gameStateFsm)
        {
            _gameStateFsm = gameStateFsm;

            //var gameData = gameDataFactory.Create(
            //    players.Next(),
            //    players.ToList(),
            //    initialTerritories,
            //    initialDeck);

            //gameStateConductor.InitializeFirstPlayerTurn(gameData);
        }

        public IPlayer CurrentPlayer => _gameStateFsm.CurrentPlayer;

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

            var isCurrentPlayerOccupyingTerritory = territory.Player == _gameStateFsm.CurrentPlayer;

            return isCurrentPlayerOccupyingTerritory;
        }
    }
}