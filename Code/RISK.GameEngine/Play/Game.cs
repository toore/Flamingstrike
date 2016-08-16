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
        bool CanFreeMove();
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        bool CanEndTurn();
        void EndTurn();
        bool IsGameOver();
        bool IsCurrentPlayerOccupyingRegion(IRegion region);
    }

    public class Game : IGame
    {
        private readonly IGameContext _gameContext;
        private readonly IGameRules _gameRules;

        public Game(IGameContext gameContext, IGameRules gameRules)
        {
            _gameContext = gameContext;
            _gameRules = gameRules;
        }

        public IPlayer CurrentPlayer => _gameContext.CurrentPlayer;

        public ITerritory GetTerritory(IRegion region)
        {
            return _gameContext.GetTerritory(region);
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return _gameContext.CanPlaceDraftArmies(region);
        }

        public int GetNumberOfArmiesToDraft()
        {
            return _gameContext.GetNumberOfArmiesToDraft();
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            _gameContext.PlaceDraftArmies(region, numberOfArmies);
        }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return _gameContext.CanAttack(attackingRegion, defendingRegion);
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _gameContext.Attack(attackingRegion, defendingRegion);
        }

        public bool CanSendArmiesToOccupy()
        {
            return _gameContext.CanSendAdditionalArmiesToOccupy();
        }

        public int GetNumberOfArmiesThatCanBeSentToOccupy()
        {
            return _gameContext.GetNumberOfAdditionalArmiesThatCanBeSentToOccupy();
        }

        public void SendArmiesToOccupy(int numberOfArmies)
        {
            _gameContext.SendAdditionalArmiesToOccupy(numberOfArmies);
        }

        public bool CanFreeMove()
        {
            return _gameContext.CanFreeMove();
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _gameContext.CanFortify(sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _gameContext.Fortify(sourceRegion, destinationRegion, armies);
        }

        public bool CanEndTurn()
        {
            return _gameContext.CanEndTurn();
        }

        public void EndTurn()
        {
            _gameContext.EndTurn();
        }

        public bool IsGameOver()
        {
            return _gameRules.IsGameOver(_gameContext.Territories);
        }

        public bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            var territory = GetTerritory(region);

            var isCurrentPlayerOccupyingTerritory = territory.Player == _gameContext.CurrentPlayer;

            return isCurrentPlayerOccupyingTerritory;
        }
    }

    public static class GameExtensions
    {
        public static bool HasArmiesToDraft(this IGame game)
        {
            return game.GetNumberOfArmiesToDraft() > 0;
        }
    }
}