using Caliburn.Micro;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;

namespace GuiWpf.ViewModels.AlternateSetup
{
    public interface IAlternateGameSetupViewModelFactory
    {
        IAlternateGameSetupViewModel Create();
    }

    public class AlternateGameSetupViewModelFactory : IAlternateGameSetupViewModelFactory
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;

        public AlternateGameSetupViewModelFactory(
            IWorldMapViewModelFactory worldMapViewModelFactory,
            IDialogManager dialogManager,
            IEventAggregator eventAggregator)
        {
            _worldMapViewModelFactory = worldMapViewModelFactory;
            _dialogManager = dialogManager;
            _eventAggregator = eventAggregator;
        }

        public IAlternateGameSetupViewModel Create()
        {
            return new AlternateGameSetupViewModel(
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }
    }
}