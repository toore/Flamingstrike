using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSettingsViewModel : IMainGameViewViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void Confirm();
    }
}