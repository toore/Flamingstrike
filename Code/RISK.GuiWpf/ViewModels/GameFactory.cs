using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels
{
    public class GameFactory : IGameFactory
    {
        private readonly ITurnFactory _turnFactory;
        private readonly IPlayerRepository _playerRepository;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(ITurnFactory turnFactory, IPlayerRepository playerRepository, IAlternateGameSetup alternateGameSetup)
        {
            _turnFactory = turnFactory;
            _playerRepository = playerRepository;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create()
        {
            return new Game(_turnFactory, _playerRepository, _alternateGameSetup);
        }
    }
}