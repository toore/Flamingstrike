using FlamingStrike.GameEngine.Attacking;

namespace FlamingStrike.GameEngine.Play.GameStates
{
    public interface IGameStateFactory
    {
        IDraftArmiesPhaseGameState CreateDraftArmiesGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, int numberOfArmiesToDraft);
        IAttackPhaseGameState CreateAttackPhaseGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, TurnConqueringAchievement turnConqueringAchievement);
        ISendArmiesToOccupyGameState CreateSendArmiesToOccupyGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, IRegion attackingRegion, IRegion occupiedRegion);
        IEndTurnGameState CreateEndTurnGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor);
        IGameOverGameState CreateGameOverGameState(IPlayer winner);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IPlayerEliminationRules _playerEliminationRules;
        private readonly IArmyDrafter _armyDrafter;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly IAttacker _attacker;
        private readonly IFortifier _fortifier;

        public GameStateFactory(
            IPlayerEliminationRules playerEliminationRules,
            IArmyDrafter armyDrafter,
            IAttacker attacker,
            ITerritoryOccupier territoryOccupier,
            IFortifier fortifier)
        {
            _playerEliminationRules = playerEliminationRules;
            _armyDrafter = armyDrafter;
            _attacker = attacker;
            _territoryOccupier = territoryOccupier;
            _fortifier = fortifier;
        }

        public IDraftArmiesPhaseGameState CreateDraftArmiesGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, int numberOfArmiesToDraft)
        {
            return new DraftArmiesPhaseGameState(gameData, gamePhaseConductor, _armyDrafter, numberOfArmiesToDraft);
        }

        public IAttackPhaseGameState CreateAttackPhaseGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, TurnConqueringAchievement turnConqueringAchievement)
        {
            return new AttackPhaseStateGameState(gameData, gamePhaseConductor, _attacker, _fortifier, _playerEliminationRules, turnConqueringAchievement);
        }

        public ISendArmiesToOccupyGameState CreateSendArmiesToOccupyGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return new SendArmiesToOccupyGameState(gameData, gamePhaseConductor, _territoryOccupier, attackingRegion, occupiedRegion);
        }

        public IEndTurnGameState CreateEndTurnGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor)
        {
            return new EndTurnGameState(gameData, gamePhaseConductor);
        }

        public IGameOverGameState CreateGameOverGameState(IPlayer winner)
        {
            return new GameOverGameState(winner);
        }
    }
}