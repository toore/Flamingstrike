using Caliburn.Micro;
using GuiWpf.Services;

namespace GuiWpf.ViewModels
{
    public class ConfirmViewModel : Screen
    {
        private readonly IScreenService _screenService;

        public ConfirmViewModel(IScreenService screenService)
        {
            _screenService = screenService;
        }

        public void Confirm()
        {
            _screenService.Confirm(this);
        }

        public void Cancel()
        {
            _screenService.Cancel(this);
        }
    }
}