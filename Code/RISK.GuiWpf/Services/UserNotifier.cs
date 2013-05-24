using Caliburn.Micro;
using GuiWpf.ViewModels;

namespace GuiWpf.Services
{
    public class UserNotifier : IUserNotifier
    {
        private readonly IWindowManager _windowManager;
        private readonly IConfirmViewModelFactory _confirmViewModelFactory;

        public UserNotifier(IWindowManager windowManager, IConfirmViewModelFactory confirmViewModelFactory)
        {
            _windowManager = windowManager;
            _confirmViewModelFactory = confirmViewModelFactory;
        }

        public bool? Confirm(string message)
        {
            var confirmViewModel = _confirmViewModelFactory.Create(message);

            return _windowManager.ShowDialog(confirmViewModel);
        }
    }
}