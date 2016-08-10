using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModelFactory
    {
        IAlternateGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup);
    }

    public class AlternateGameSetupViewModelFactory : IAlternateGameSetupViewModelFactory
    {
        private readonly IGameFactory _gameFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITaskEx _taskEx;

        public AlternateGameSetupViewModelFactory(
            IGameFactory gameFactory,
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator,
            ITaskEx taskEx)
        {
            _gameFactory = gameFactory;
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
            _taskEx = taskEx;
        }

        public IAlternateGameSetupViewModel Create(IAlternateGameSetup alternateGameSetup)
        {
            return new AlternateGameSetupViewModel(
                _gameFactory,
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator,
                alternateGameSetup,
                _taskEx);
        }
    }
}