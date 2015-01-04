using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameSettingsViewModel : IMainViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void Confirm();
    }
}