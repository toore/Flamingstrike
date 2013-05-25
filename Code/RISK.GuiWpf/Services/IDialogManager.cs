namespace GuiWpf.Services
{
    public interface IDialogManager
    {
        bool? ConfirmEndGame();
    }

    public class DialogManager : IDialogManager
    {
        private readonly IUserNotifier _userNotifier;
        private readonly IResourceManagerWrapper _resourceManagerWrapper;

        public DialogManager(IUserNotifier userNotifier, IResourceManagerWrapper resourceManagerWrapper)
        {
            _userNotifier = userNotifier;
            _resourceManagerWrapper = resourceManagerWrapper;
        }

        public bool? ConfirmEndGame()
        {
            return _userNotifier.Confirm(
                _resourceManagerWrapper.GetString("ARE_YOU_SURE_YOU_WANT_TO_END_GAME"),
                _resourceManagerWrapper.GetString("END_GAME"),
                _resourceManagerWrapper.GetString("YES"),
                _resourceManagerWrapper.GetString("NO"));
        }
    }
}