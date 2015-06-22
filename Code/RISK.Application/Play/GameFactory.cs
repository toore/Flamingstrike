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
        private readonly IGameboardRules _gameboardRules;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;
        private readonly ITerritoryConverter _territoryConverter;

        public GameFactory(IGameboardRules gameboardRules, ICardFactory cardFactory, IBattle battle, ITerritoryConverter territoryConverter)
        {
            _gameboardRules = gameboardRules;
            _cardFactory = cardFactory;
            _battle = battle;
            _territoryConverter = territoryConverter;
        }

        public Game Create(IGameSetup gameSetup)
        {
            var game = new Game(gameSetup, _gameboardRules, _cardFactory, _battle, _territoryConverter);
            return game;
        }
    }
}