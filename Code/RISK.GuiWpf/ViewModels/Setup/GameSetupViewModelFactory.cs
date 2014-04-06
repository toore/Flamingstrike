using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IGameSettingStateConductor gameSettingStateConductor);
    }

    public class GameSetupViewModelFactory : IGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactoryWorker _gameFactoryWorker;

        public GameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            IUserInteractor userInteractor,
            IGameFactoryWorker gameFactoryWorker)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _userInteractor = userInteractor;
            _gameFactoryWorker = gameFactoryWorker;
        }

        public IGameSetupViewModel Create(IGameSettingStateConductor gameSettingStateConductor)
        {
            return new GameSetupViewModel(_worldMapViewModelFactory, gameSettingStateConductor, _dialogManager, _eventAggregator, _userInteractor, _gameFactoryWorker);
        }
    }
}