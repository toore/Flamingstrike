namespace GuiWpf.Services
{
    public interface IUserNotifier
    {
        bool? Confirm(string message, string displayName, string confirmText, string abortText);
    }
}