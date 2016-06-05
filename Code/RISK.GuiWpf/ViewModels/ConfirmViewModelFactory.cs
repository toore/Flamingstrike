using GuiWpf.Services;

namespace GuiWpf.ViewModels
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
                abortText = ResourceManager.Instance.GetString("CANCEL");
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