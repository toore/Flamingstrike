using FlamingStrike.UI.WPF.Properties;

namespace FlamingStrike.UI.WPF.Services
{
    public interface IDialogManager
    {
        bool? ConfirmEndGame();
        void ShowGameOverDialog(string winnerName);
    }

    public class DialogManager : IDialogManager
    {
        private readonly IUserNotifier _userNotifier;

        public DialogManager(IUserNotifier userNotifier)
        {
            _userNotifier = userNotifier;
        }

        public bool? ConfirmEndGame()
        {
            var message = Resources.ARE_YOU_SURE_YOU_WANT_TO_END_GAME;
            var displayName = Resources.END_GAME;
            var confirmText = Resources.YES;
            var abortText = Resources.NO;

            return _userNotifier.Confirm(message, displayName, confirmText, abortText);
        }

        public void ShowGameOverDialog(string winnerName)
        {
            var message = string.Format(Resources.ARG0_IS_THE_WINNER, winnerName);
            var displayName = Resources.GAME_OVER;

            _userNotifier.Notify(message, displayName);
        }
    }
}