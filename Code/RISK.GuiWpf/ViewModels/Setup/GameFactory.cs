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
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IPlayers _players;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameFactory(IInteractionStateFactory interactionStateFactory, IPlayers players, IAlternateGameSetup alternateGameSetup)
        {
            _interactionStateFactory = interactionStateFactory;
            _players = players;
            _alternateGameSetup = alternateGameSetup;
        }

        public IGame Create(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            return new Game(_interactionStateFactory, _players, _alternateGameSetup, gameInitializerLocationSelector);
        }
    }
}