using RISK.Application;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactory
    {
        IGame Create(ITerritorySelector territorySelector);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly IPlayers _players;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly ICardFactory _cardFactory;

        public GameFactory(
            IAlternateGameSetup alternateGameSetup, 
            IInteractionStateFactory interactionStateFactory, 
            IStateControllerFactory stateControllerFactory, 
            IPlayers players, 
            ICardFactory cardFactory)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _players = players;
            _alternateGameSetup = alternateGameSetup;
            _cardFactory = cardFactory;
        }

        public IGame Create(ITerritorySelector territorySelector)
        {
            var worldMap = _alternateGameSetup.InitializeWorldMap(territorySelector);

            return new Game(_interactionStateFactory, _stateControllerFactory , _players, worldMap, _cardFactory);
        }
    }
}