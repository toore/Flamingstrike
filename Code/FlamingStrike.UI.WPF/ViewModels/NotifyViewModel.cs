using Caliburn.Micro;

namespace FlamingStrike.UI.WPF.ViewModels
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