namespace GuiWpf.Services
{
    public interface IUserNotifier
    {
        bool? Confirm(string message);
    }
}