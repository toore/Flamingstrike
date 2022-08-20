using System.Threading.Tasks;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.ViewModels;

namespace FlamingStrike.UI.WPF.Services
{
    public interface IUserNotifier
    {
        Task<bool?> ConfirmAsync(string message, string displayName, string confirmText, string abortText);
        Task NotifyAsync(string message, string displayName);
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

        public async Task<bool?> ConfirmAsync(string message, string displayName, string confirmText, string abortText)
        {
            var confirmViewModel = _confirmViewModelFactory.Create(message, displayName, confirmText, abortText);
            
            return await _windowManager.ShowDialogAsync(confirmViewModel);
        }

        public async Task NotifyAsync(string message, string displayName)
        {
            var notifyViewModel = new NotifyViewModel
                {
                    Message = message,
                    DisplayName = displayName
                };
            
            await _windowManager.ShowDialogAsync(notifyViewModel);
        }
    }
}