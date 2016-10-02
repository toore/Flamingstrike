using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Interaction;
using RISK.GameEngine;

namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameplayViewModelFactory
    {
        IGameplayViewModel Create();
    }

    public class GameplayViewModelFactory : IGameplayViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly IGameOverViewModelFactory _gameOverViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IInteractionStateFactory _interactionStateFactory;

        public GameplayViewModelFactory(
            IInteractionStateFactory interactionStateFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IWindowManager windowManager,
            IGameOverViewModelFactory gameOverViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _interactionStateFactory = interactionStateFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _windowManager = windowManager;
            _gameOverViewModelFactory = gameOverViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IGameplayViewModel Create()
        {
            return new GameplayViewModel(
                _interactionStateFactory,
                _worldMapViewModelFactory,
                _windowManager,
                _gameOverViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}