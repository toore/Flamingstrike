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

        public bool? Confirm(string message, string displayName, string confirmText, string abortText)
        {
            var confirmViewModel = _confirmViewModelFactory.Create(message, displayName, confirmText, abortText);

            return _windowManager.ShowDialog(confirmViewModel);
        }
    }
}