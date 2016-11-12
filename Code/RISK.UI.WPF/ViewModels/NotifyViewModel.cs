using Caliburn.Micro;

namespace RISK.UI.WPF.ViewModels
{
    public class NotifyViewModel : Screen
    {
        public string Message { get; set; }

        public void Close()
        {
            TryClose();
        }
    }
}