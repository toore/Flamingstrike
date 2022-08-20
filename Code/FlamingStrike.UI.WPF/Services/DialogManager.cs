using System.Threading.Tasks;
using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.Services
{
    public interface IDialogManager
    {
        Task<bool?> ConfirmEndGameAsync();
        Task ShowGameOverDialogAsync(string winnerName);
    }

    public class DialogManager : IDialogManager
    {
        private readonly IUserNotifier _userNotifier;

        public DialogManager(IUserNotifier userNotifier)
        {
            _userNotifier = userNotifier;
        }

        public async Task<bool?> ConfirmEndGameAsync()
        {
            var message = Resources.ARE_YOU_SURE_YOU_WANT_TO_END_THE_GAME;
            var displayName = Resources.END_GAME;
            var confirmText = Resources.YES;
            var abortText = Resources.NO;

            return await _userNotifier.ConfirmAsync(message, displayName, confirmText, abortText);
        }

        public async Task ShowGameOverDialogAsync(string winnerName)
        {
            var message = string.Format(Resources.ARG0_IS_THE_WINNER, winnerName);
            var displayName = Resources.GAME_OVER;

            await _userNotifier.NotifyAsync(message, displayName);
        }
    }
}