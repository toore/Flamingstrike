using RISK.Application.Play;
using RISK.Application.Play.GamePhases;

namespace RISK.Tests.Application.GameStates
{
    public class SendInArmiesToOccupyGameStateTests : GameStateTestsBase
    {
        protected override IGameState Create(GameData gameData)
        {
            return new SendInArmiesToOccupyGameState(gameData);
        }
    }
}