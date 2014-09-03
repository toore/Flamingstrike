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
            return _userNotifier.Confirm(
                LanguageResources.Instance.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME"),
                LanguageResources.Instance.GetString("END_GAME"),
                LanguageResources.Instance.GetString("YES"),
                LanguageResources.Instance.GetString("NO"));
        }
    }
}