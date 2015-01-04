using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create();
    }

    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactory _gameFactory;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactory gameFactory)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactory = gameFactory;
        }

        public IGameSetupViewModel Create()
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, _dialogManager, _eventAggregator, _userInteractor, _gameFactory);
        }
    }
}