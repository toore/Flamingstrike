using GuiWpf.Services;

namespace GuiWpf.ViewModels
{
    public class ConfirmViewModelFactory : IConfirmViewModelFactory
    {
        private readonly IScreenService _screenService;

        public ConfirmViewModelFactory(IScreenService screenService)
        {
            _screenService = screenService;
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

            return new ConfirmViewModel(_screenService)
                {
                    Message = message,
                    DisplayName = displayName,
                    ConfirmText = confirmText,
                    AbortText = abortText
                };
        }
    }
}