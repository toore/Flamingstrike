using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameSettingsViewModel : IMainGameViewViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void Confirm();
    }
}