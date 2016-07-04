using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModelFactory
    {
        IGameboardViewModel Create(IGame game);
    }

    public class GameboardViewModelFactory : IGameboardViewModelFactory
    {
        private readonly IInteractionStateFsm _interactionStateFsm;
        private readonly IRegions _regions;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public GameboardViewModelFactory(
            IInteractionStateFsm interactionStateFsm,
            IInteractionStateFactory interactionStateFactory,
            IRegions regions,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _interactionStateFsm = interactionStateFsm;
            _interactionStateFactory = interactionStateFactory;
            _regions = regions;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameboardViewModel Create(IGame game)
        {
            return new GameboardViewModel(
                game,
                _interactionStateFsm,
                _interactionStateFactory,
                _regions,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}