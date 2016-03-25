using RISK.Application.Play.Attacking;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft);
        IGameState CreateAttackGameState(GameData gameData);
        IGameState CreateSendInArmiesToOccupyGameState(GameData gameData);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBattle _battle;
        private readonly IArmyDraftCalculator _armyDraftCalculator;
        private readonly ITerritoryUpdater _territoryUpdater;

        public GameStateFactory(IBattle battle, IArmyDraftCalculator armyDraftCalculator, ITerritoryUpdater territoryUpdater)
        {
            _battle = battle;
            _armyDraftCalculator = armyDraftCalculator;
            _territoryUpdater = territoryUpdater;
        }

        public IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(this, _territoryUpdater, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(GameData gameData)
        {
            return new AttackGameState(this, _battle, _armyDraftCalculator, gameData);
        }

        public IGameState CreateSendInArmiesToOccupyGameState(GameData gameData)
        {
            return new SendInArmiesToOccupyGameState(gameData);
        }

        public IGameState CreateGameOverGameState(GameData gameData)
        {
            return new GameOverGameState(gameData);
        }
    }
}