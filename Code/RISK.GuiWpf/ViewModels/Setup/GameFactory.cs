using System;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Settings;
using RISK.Application.GamePlay;
using RISK.Application.GamePlay.Battling;
using RISK.Application.GameSetup;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameFactory
    {
        IGameAdapter Create(ITerritoryRequestHandler territoryRequestHandler);
    }

    public class GameFactory : IGameFactory
    {
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IStateControllerFactory _stateControllerFactory;
        private readonly PlayerRepository _playerRepository;
        private readonly ICardFactory _cardFactory;
        private readonly IBattle _battle;

        public GameFactory(
            IInteractionStateFactory interactionStateFactory,
            IStateControllerFactory stateControllerFactory,
            PlayerRepository playerRepository,
            ICardFactory cardFactory,
            IBattle battle)
        {
            _interactionStateFactory = interactionStateFactory;
            _stateControllerFactory = stateControllerFactory;
            _playerRepository = playerRepository;
            _cardFactory = cardFactory;
            _battle = battle;
        }

        public IGameAdapter Create(ITerritoryRequestHandler territoryRequestHandler)
        {
            //var gameboard = _alternateGameSetup.Initialize(territoryRequestHandler);
            var players = _playerRepository.GetAll();

            //var game = new Game(players, gameboard, _cardFactory, _battle);

            //return new GameAdapter(_interactionStateFactory, _stateControllerFactory, game);

            throw new NotImplementedException();
        }
    }
}