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
        private readonly ITerritoryFactory _territoryFactory;
        private readonly IPlayerFactory _playerFactory;

        public GameFactory(IGameRules gameRules, ICardFactory cardFactory, IBattle battle, ITerritoryFactory territoryFactory, IPlayerFactory playerFactory)
        {
            _gameRules = gameRules;
            _cardFactory = cardFactory;
            _battle = battle;
            _territoryFactory = territoryFactory;
            _playerFactory = playerFactory;
        }

        public IGame Create(IGamePlaySetup gamePlaySetup)
        {
            var territories = _territoryFactory.Create(gamePlaySetup.Territories);
            var players = _playerFactory.Create(gamePlaySetup.Players);

            var game = new Game(players, territories, _gameRules, _cardFactory, _battle);

            return game;
        }
    }
}