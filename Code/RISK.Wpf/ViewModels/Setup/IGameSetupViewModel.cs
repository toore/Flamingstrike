using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModel : IMainGameViewViewModel
    {
        ObservableCollection<PlayerSetupViewModel> Players { get; }
        void OnConfirm();
    }
}