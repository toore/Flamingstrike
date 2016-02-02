using RISK.Application.Play.GamePhases;
using RISK.Application.Setup;

namespace RISK.Application.Play
{
    public interface IGameFactory
    {
        IGame Create(IGamePlaySetup gamePlaySetup);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly INewArmiesDraftCalculator _newArmiesDraftCalculator;

        public GameFactory(IGameStateFactory gameStateFactory, INewArmiesDraftCalculator newArmiesDraftCalculator)
        {
            _gameStateFactory = gameStateFactory;
            _newArmiesDraftCalculator = newArmiesDraftCalculator;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(_gameStateFactory, _newArmiesDraftCalculator, gamePlaySetup.Players, gamePlaySetup.Territories);

            return game;
        }
    }
}