using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Settings
{
    public interface IGamePreparationViewModel : IMainViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void Confirm();
    }
}