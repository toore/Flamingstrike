using System.Collections.Generic;
using RISK.GameEngine.Attacking;

namespace RISK.GameEngine.Play.GameStates
{
    public interface IGameStateFactory
    {
        IDraftArmiesPhaseGameState CreateDraftArmiesGameState(IPlayer currentPlayer, ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, int numberOfArmiesToDraft);
        IAttackPhaseGameState CreateAttackPhaseGameState(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IDeck deck, ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, TurnConqueringAchievement turnConqueringAchievement);
        ISendArmiesToOccupyGameState CreateSendArmiesToOccupyGameState(ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, IRegion attackingRegion, IRegion occupiedRegion);
        IEndTurnGameState CreateEndTurnGameState(IGamePhaseConductor gamePhaseConductor);
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

        public IDraftArmiesPhaseGameState CreateDraftArmiesGameState(IPlayer currentPlayer, ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, int numberOfArmiesToDraft)
        {
            return new DraftArmiesPhaseGameState(currentPlayer, territoriesContext, gamePhaseConductor, _armyDrafter, numberOfArmiesToDraft);
        }

        public IAttackPhaseGameState CreateAttackPhaseGameState(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IDeck deck, ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, TurnConqueringAchievement turnConqueringAchievement)
        {
            return new AttackPhaseStateGameState(currentPlayer, players, territoriesContext, deck, gamePhaseConductor, _attacker, _fortifier, _playerEliminationRules, turnConqueringAchievement);
        }

        public ISendArmiesToOccupyGameState CreateSendArmiesToOccupyGameState(ITerritoriesContext territoriesContext, IGamePhaseConductor gamePhaseConductor, IRegion attackingRegion, IRegion occupiedRegion)
        {
            return new SendArmiesToOccupyGameState(territoriesContext, gamePhaseConductor, _territoryOccupier, attackingRegion, occupiedRegion);
        }

        public IEndTurnGameState CreateEndTurnGameState(IGamePhaseConductor gamePhaseConductor)
        {
            return new EndTurnGameState(gamePhaseConductor);
        }

        public IGameOverGameState CreateGameOverGameState(IPlayer winner)
        {
            return new GameOverGameState(winner);
        }
    }
}