using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGameInitializationViewModel : IMainViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void Confirm();
    }
}