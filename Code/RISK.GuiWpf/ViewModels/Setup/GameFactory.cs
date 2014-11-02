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
        private readonly ICardFactory _cardFactory;

        public GameFactory(IInteractionStateFactory interactionStateFactory, IPlayers players, IAlternateGameSetup alternateGameSetup, ICardFactory cardFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _players = players;
            _alternateGameSetup = alternateGameSetup;
            _cardFactory = cardFactory;
        }

        public IGame Create(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            var worldMap = _alternateGameSetup.Initialize(gameInitializerLocationSelector);

            return new Game(_interactionStateFactory, new StateControllerFactory() , _players, worldMap, _cardFactory);
        }
    }
}