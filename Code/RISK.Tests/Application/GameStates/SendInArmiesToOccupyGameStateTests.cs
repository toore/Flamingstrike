using NSubstitute;
using RISK.Application.Play;
using RISK.Application.Play.GamePhases;

namespace RISK.Tests.Application.GameStates
{
    public class SendInArmiesToOccupyGameStateTests : GameStateTestsBase
    {
        private readonly IGameStateFactory _gameStateFactory;

        public SendInArmiesToOccupyGameStateTests()
        {
            _gameStateFactory = Substitute.For<IGameStateFactory>();
        }

        protected override IGameState Create(GameData gameData)
        {
            return new SendInArmiesToOccupyGameState(_gameStateFactory, gameData);
        }
    }
}