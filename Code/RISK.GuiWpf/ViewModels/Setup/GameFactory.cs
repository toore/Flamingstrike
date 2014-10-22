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
        private readonly ITurnStateFactory _turnStateFactory;
        private readonly IPlayers _players;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(ITurnStateFactory turnStateFactory, IPlayers players, IAlternateGameSetup alternateGameSetup)
        {
            _turnStateFactory = turnStateFactory;
            _players = players;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            return new Game(_turnStateFactory, _players, _alternateGameSetup, gameInitializerLocationSelector);
        }
    }
}