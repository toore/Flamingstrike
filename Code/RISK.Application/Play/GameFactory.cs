using RISK.Application.Play.Attacking;
using RISK.Application.Setup;

namespace RISK.Application.Play
{
    public interface IGameFactory
    {
        IGame Create(IGamePlaySetup gamePlaySetup);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IGameRules _gameRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        public GameFactory(IGameRules gameRules, ICardFactory cardFactory, IBattle battle)
        {
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(gamePlaySetup.Players, gamePlaySetup.Territories, _gameRules, _cardFactory, _battle);

            return game;
        }
    }
}