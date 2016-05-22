using RISK.Core;

namespace RISK.GameEngine.Play.GamePhases
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
        private readonly IArmyDrafter _armyDrafter;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;

        public GameStateFactory(IGameDataFactory gameDataFactory, IArmyDrafter armyDrafter, ITerritoryOccupier territoryOccupier, IAttacker attacker, IFortifier fortifier)
        {
            _gameDataFactory = gameDataFactory;
            _attacker = attacker;
            _fortifier = fortifier;
            _armyDrafter = armyDrafter;
            _territoryOccupier = territoryOccupier;
        }

        public IGameState CreateDraftArmiesGameState(IGameStateConductor gameStateConductor, GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(gameStateConductor, _gameDataFactory, _armyDrafter, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(IGameStateConductor gameStateConductor, GameData gameData)
        {
            return new AttackGameState(gameStateConductor, _gameDataFactory, _attacker, _fortifier, gameData);
        }

        public IGameState CreateSendArmiesToOccupyGameState(IGameStateConductor gameStateConductor, GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return new SendArmiesToOccupyGameState(gameStateConductor, _gameDataFactory, _territoryOccupier, gameData, attackingRegion, occupiedRegion);
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