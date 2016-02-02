using System;
using System.Linq;
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
        private readonly INewArmiesDraftCalculator _newArmiesDraftCalculator;

        public GameStateFactory(IBattle battle, INewArmiesDraftCalculator newArmiesDraftCalculator)
        {
            _battle = battle;
            _newArmiesDraftCalculator = newArmiesDraftCalculator;
        }

        public IGameState CreateDraftArmiesGameState(GameData gameData)
        {
            var numberOfArmiesToDraft = _newArmiesDraftCalculator.Calculate(gameData.CurrentPlayer, gameData.Territories);

            return new DraftArmiesGameState(this, gameData, numberOfArmiesToDraft);
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