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
            var message = ResourceManager.Instance.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME");
            var displayName = ResourceManager.Instance.GetString("END_GAME");
            var confirmText = ResourceManager.Instance.GetString("YES");
            var abortText = ResourceManager.Instance.GetString("NO");

            return _userNotifier.Confirm(message, displayName, confirmText, abortText);
        }
    }
}