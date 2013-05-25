using GuiWpf.Services;

namespace GuiWpf.ViewModels
{
    public class ConfirmViewModelFactory : IConfirmViewModelFactory
    {
        private readonly IScreenService _screenService;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;

        public ConfirmViewModelFactory(IScreenService screenService, IResourceManagerWrapper resourceManagerWrapper)
        {
            _screenService = screenService;
            _resourceManagerWrapper = resourceManagerWrapper;
        }

        public ConfirmViewModel Create(string message, string confirmText, string abortText)
        {
            if (confirmText == null)
            {
                confirmText = "OK";
            }

            if (abortText == null)
            {
                abortText = _resourceManagerWrapper.GetString("CANCEL");
            }

            return new ConfirmViewModel(_screenService)
                {
                    Message = message,
                    ConfirmText = confirmText,
                    AbortText = abortText
                };
        }
    }
}