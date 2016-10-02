namespace RISK.UI.WPF.ViewModels
{
    public interface IConfirmViewModelFactory
    {
        ConfirmViewModel Create(string message, string displayName = "", string confirmText = null, string abortText = null);
    }
}