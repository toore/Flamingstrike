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
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        public GameFactory(ICardFactory cardFactory, IBattle battle)
        {
            _cardFactory = cardFactory;
            _battle = battle;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var game = new Game(gamePlaySetup.Players, gamePlaySetup.Territories, _cardFactory, _battle);

            return game;
        }
    }
}