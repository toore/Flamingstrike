using RISK.Domain;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public class GameFactory : IGameFactory
    {
        private readonly ITurnFactory _turnFactory;
        private readonly IPlayers _players;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(ITurnFactory turnFactory, IPlayers players, IAlternateGameSetup alternateGameSetup)
        {
            _turnFactory = turnFactory;
            _players = players;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create(ILocationSelector locationSelector)
        {
            return new Game(_turnFactory, _players, _alternateGameSetup, locationSelector);
        }
    }
}