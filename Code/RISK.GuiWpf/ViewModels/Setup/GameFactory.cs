using System.Linq;
using RISK.Application;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactory
    {
        IGameAdapter Create(ITerritorySelector territorySelector);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly PlayerRepository _playerRepository;
        private readonly IAlternateGameSetup _alternateGameSetup;
        private readonly ICardFactory _cardFactory;
        private readonly IBattleCalculator _battleCalculator;

        public GameFactory(
            IAlternateGameSetup alternateGameSetup,
            IInteractionStateFactory interactionStateFactory,
            IStateControllerFactory stateControllerFactory,
            PlayerRepository playerRepository,
            ICardFactory cardFactory,
            IBattleCalculator battleCalculator)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _playerRepository = playerRepository;
            _alternateGameSetup = alternateGameSetup;
            _cardFactory = cardFactory;
            _battleCalculator = battleCalculator;
        }

        public IGameAdapter Create(ITerritorySelector territorySelector)
        {
            var worldMap = _alternateGameSetup.InitializeWorldMap(territorySelector);
            var players = _playerRepository.GetAll()
                .OrderBy(x=>x.PlayerOrderIndex);

            var game = new Game(players, worldMap, _cardFactory, _battleCalculator);
            
            return new GameAdapter(_interactionStateFactory, _stateControllerFactory, game);
        }
    }
}