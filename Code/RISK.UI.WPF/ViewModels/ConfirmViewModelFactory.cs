using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;

namespace RISK.UI.WPF.ViewModels
{
    public class ConfirmViewModelFactory : IConfirmViewModelFactory
    {
        private readonly IScreenConfirmationService _screenConfirmationService;

        public ConfirmViewModelFactory(IScreenConfirmationService screenConfirmationService)
        {
            _screenConfirmationService = screenConfirmationService;
        }

        public ConfirmViewModel Create(string message, string displayName, string confirmText, string abortText)
        {
            if (confirmText == null)
            {
                confirmText = "OK";
            }

            if (abortText == null)
            {
                abortText = Resources.CANCEL;
            }

            return new ConfirmViewModel(_screenConfirmationService)
                {
                    Message = message,
                    DisplayName = displayName,
                    ConfirmText = confirmText,
                    AbortText = abortText
                };
        }
    }
}