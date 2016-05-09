using RISK.Application.Play.Attacking;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(IGameStateConductor gameStateConductor, GameData gameData, int numberOfArmiesToDraft);
        IGameState CreateAttackGameState(IGameStateConductor gameStateConductor, GameData gameData);
        IGameState CreateSendArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData, IRegion attackingRegion, IRegion occupiedRegion);
        IGameState CreateFortifyState(IGameStateConductor gameStateConductor, GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify);
        IGameState CreateGameOverGameState(GameData gameData);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IGameDataFactory _gameDataFactory;
        private readonly IBattle _battle;
        private readonly IArmyModifier _armyModifier;

        public GameStateFactory(IGameDataFactory gameDataFactory, IBattle battle, IArmyModifier armyModifier)
        {
            _gameDataFactory = gameDataFactory;
            _battle = battle;
            _armyModifier = armyModifier;
        }

        public IGameState CreateDraftArmiesGameState(IGameStateConductor gameStateConductor, GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(gameStateConductor, _gameDataFactory, _armyModifier, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(IGameStateConductor gameStateConductor, GameData gameData)
        {
            return new AttackGameState(gameStateConductor, _gameDataFactory, _battle, gameData);
        }

        public IGameState CreateSendArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return new SendArmiesToOccupyGameState(gameStateConductor, _gameDataFactory, _armyModifier, gameData, attackingRegion, occupiedRegion);
        }

        public IGameState CreateFortifyState(IGameStateConductor gameStateConductor, GameData gameData, IRegion sourceRegion, IRegion destinationRegion, int numberOfArmiesToFortify)
        {
            return new FortifyGameState(gameStateConductor, gameData);
        }

        public IGameState CreateGameOverGameState(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}