using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using RISK.Domain.Repositories;

namespace GuiWpf.ViewModels.Setup
{
    public class GameFactory : IGameFactory
    {
        private readonly ITurnFactory _turnFactory;
        private readonly IPlayerProvider _playerProvider;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(ITurnFactory turnFactory, IPlayerProvider playerProvider, IAlternateGameSetup alternateGameSetup)
        {
            _turnFactory = turnFactory;
            _playerProvider = playerProvider;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create(ILocationSelector locationSelector)
        {
            return new Game(_turnFactory, _playerProvider, _alternateGameSetup, locationSelector);
        }
    }
}