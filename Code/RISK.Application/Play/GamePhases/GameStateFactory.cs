using System;
using RISK.Application.Play.Attacking;

namespace RISK.Application.Play.GamePhases
{
    public interface IGameStateFactory
    {
        IGameState CreateDraftArmiesGameState(GameData gameData);
    }

    public class GameStateFactory : IGameStateFactory
    {
        private readonly IBattle _battle;

        public GameStateFactory(IBattle battle)
        {
            _battle = battle;
        }

        public IGameState CreateDraftArmiesGameState(GameData gameData)
        {
            return new DraftArmiesGameState(this, gameData);
        }

        public IGameState CreateAttackState(GameData gameData)
        {
            return new AttackGameState(this, gameData, _battle);
        }

        public IGameState CreateSendInArmiesToOccupyState(GameData gameData)
        {
            throw new NotImplementedException();
        }
    }
}