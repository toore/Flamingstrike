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

        public ConfirmViewModel Create(string message)
        {
            return new ConfirmViewModel(_screenService, message);
        }
    }
}