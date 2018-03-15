namespace FlamingStrike.GameEngine.Play.GameStates
{
    public interface IGameStateFactory
    {
        IAttackPhaseGameState CreateAttackPhaseGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, ConqueringAchievement conqueringAchievement);
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

        public IAttackPhaseGameState CreateAttackPhaseGameState(GameData gameData, IGamePhaseConductor gamePhaseConductor, ConqueringAchievement conqueringAchievement)
        {
            return new AttackPhaseStateGameState(gameData, gamePhaseConductor, _attacker, _fortifier, _playerEliminationRules, conqueringAchievement);
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