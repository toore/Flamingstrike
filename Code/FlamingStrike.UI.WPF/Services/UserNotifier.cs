using Caliburn.Micro;
using FlamingStrike.UI.WPF.ViewModels;

namespace FlamingStrike.UI.WPF.Services
{
    public interface IUserNotifier
    {
        bool? Confirm(string message, string displayName, string confirmText, string abortText);
        void Notify(string message, string displayName);
    }

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

        public void Notify(string message, string displayName)
        {
            var notifyViewModel = new NotifyViewModel
                {
                    Message = message,
                    DisplayName = displayName
                };
            
            _windowManager.ShowDialog(notifyViewModel);
        }
    }
}