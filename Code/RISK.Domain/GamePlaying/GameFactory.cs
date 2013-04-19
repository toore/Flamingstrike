using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class GameFactory : IGameFactory 
    {
        private readonly IWorldMap _worldMap;
        private readonly ITurnFactory _turnFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(IWorldMap worldMap, ITurnFactory turnFactory, IPlayerRepository playerRepository, IAlternateGameSetup alternateGameSetup)
        {
            _worldMap = worldMap;
            _turnFactory = turnFactory;
            _playerRepository = playerRepository;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create()
        {
            return new Game(_worldMap, _turnFactory, _playerRepository, _alternateGameSetup);
        }
    }
}