namespace GuiWpf.Services
{
    public interface IDialogManager
    {
        bool? ConfirmEndGame();
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
            var message = LanguageResources.Instance.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME");
            var displayName = LanguageResources.Instance.GetString("END_GAME");
            var confirmText = LanguageResources.Instance.GetString("YES");
            var abortText = LanguageResources.Instance.GetString("NO");

            return _userNotifier.Confirm(message, displayName, confirmText, abortText);
        }
    }
}