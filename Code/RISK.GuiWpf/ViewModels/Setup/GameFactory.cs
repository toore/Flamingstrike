using RISK.Domain;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactory
    {
        IGame Create(IGameInitializerLocationSelector gameInitializerLocationSelector);
    }

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

        public IGame Create(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            return new Game(_turnFactory, _players, _alternateGameSetup, gameInitializerLocationSelector);
        }
    }
}