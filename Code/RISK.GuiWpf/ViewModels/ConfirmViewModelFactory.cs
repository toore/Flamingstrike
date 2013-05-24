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

        public ConfirmViewModel Create()
        {
            return new ConfirmViewModel(_screenService);
        }
    }
}