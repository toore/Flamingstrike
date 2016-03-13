using System;
using System.Linq;
using RISK.Application.Play.Attacking;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBattle _battle;
        private readonly IArmyDraftCalculator _armyDraftCalculator;

        public GameStateFactory(IBattle battle, IArmyDraftCalculator armyDraftCalculator)
        {
            _battle = battle;
            _armyDraftCalculator = armyDraftCalculator;
        }

        public IGameState CreateDraftArmiesGameState(GameData gameData, int numberOfArmiesToDraft)
        {
            return new DraftArmiesGameState(this, gameData, numberOfArmiesToDraft);
        }

        public IGameState CreateAttackGameState(GameData gameData)
        {
            return new AttackGameState(this, gameData, _battle, _armyDraftCalculator);
        }

        public IGameState CreateSendInArmiesToOccupyGameState(GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}