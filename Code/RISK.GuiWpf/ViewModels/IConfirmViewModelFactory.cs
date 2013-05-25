namespace GuiWpf.ViewModels
{
    public interface IConfirmViewModelFactory
    {
        ConfirmViewModel Create(string message, string confirmText = null, string abortText = null);
    }
}