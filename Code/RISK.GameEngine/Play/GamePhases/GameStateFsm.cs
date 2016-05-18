using RISK.Core;
using RISK.GameEngine.Extensions;

namespace RISK.GameEngine.Play.GamePhases
{
    public interface IGameStateFsm
    {
        void Set(IGameState gameState);
        IPlayer CurrentPlayer { get; }
        ITerritory GetTerritory(IRegion region);
        bool CanPlaceDraftArmies(IRegion region);
        int GetNumberOfArmiesToDraft();
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanSendAdditionalArmiesToOccupy();
        int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        void EndTurn();
    }

    public class GameStateFsm : IGameStateFsm
    {
        private IGameState _currentState;

        public void Set(IGameState gameState)
        {
            _currentState = gameState;
        }

        public IPlayer CurrentPlayer => _currentState.CurrentPlayer;

        public ITerritory GetTerritory(IRegion region)
        {
            return _currentState.Territories.GetTerritory(region);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return _currentState.CanPlaceDraftArmies(region);
        }

        public int GetNumberOfArmiesToDraft()
        {
            return _currentState.GetNumberOfArmiesToDraft();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _currentState.PlaceDraftArmies(region, numberOfArmies);
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return _currentState.CanAttack(attackingRegion, defendingRegion);
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _currentState.Attack(attackingRegion, defendingRegion);
        }

        public bool CanSendAdditionalArmiesToOccupy()
        {
            return _currentState.CanSendAdditionalArmiesToOccupy();
        }

        public int GetNumberOfAdditionalArmiesThatCanBeSentToOccupy()
        {
            return _currentState.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            _currentState.SendAdditionalArmiesToOccupy(numberOfArmies);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _currentState.CanFortify(sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _currentState.Fortify(sourceRegion, destinationRegion, armies);
        }

        public void EndTurn()
        {
            _currentState.EndTurn();
        }
    }
}