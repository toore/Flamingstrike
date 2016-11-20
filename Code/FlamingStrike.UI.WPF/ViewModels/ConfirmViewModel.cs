using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;

namespace FlamingStrike.UI.WPF.ViewModels
{
    public class ConfirmViewModel : Screen
    {
        private readonly IScreenConfirmationService _screenConfirmationService;

        public ConfirmViewModel(IScreenConfirmationService screenConfirmationService)
        {
            _screenConfirmationService = screenConfirmationService;
        }

        public string Message { get; set; }

        public string ConfirmText { get; set; }
        public string AbortText { get; set; }

        public void Confirm()
        {
            _screenConfirmationService.Confirm(this);
        }

        public void Cancel()
        {
            _screenConfirmationService.Cancel(this);
        }
    }
}