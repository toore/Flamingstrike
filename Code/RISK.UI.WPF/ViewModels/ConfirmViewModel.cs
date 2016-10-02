using Caliburn.Micro;
using RISK.UI.WPF.Services;

namespace RISK.UI.WPF.ViewModels
{
    public interface IConfirmViewModel
    {
        string Message { get; }
        string ConfirmText { get; }
        string AbortText { get; }
    }

    public class ConfirmViewModel : Screen, IConfirmViewModel
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