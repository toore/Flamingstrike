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
        private readonly ITerritoryConverter _territoryConverter;

        public GameFactory(IGameRules gameRules, ICardFactory cardFactory, IBattle battle, ITerritoryConverter territoryConverter)
        {
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;
            _territoryConverter = territoryConverter;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var gameboardTerritories = _territoryConverter.Convert(gamePlaySetup.GameboardSetupTerritories);

            var game = new Game(gamePlaySetup.Players, gameboardTerritories, _gameRules, _cardFactory, _battle);

            return game;
        }
    }
}