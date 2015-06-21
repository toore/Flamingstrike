using RISK.Application.Play.Battling;
using RISK.Application.Setup;

namespace RISK.Application.Play
{
    public interface IGameFactory
    {
        Game Create(IGameSetup gameSetup);
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

        public Game Create(IGameSetup gameSetup)
        {
            var game = new Game(_cardFactory ,_battle, gameSetup);
            return game;
        }
    }
}