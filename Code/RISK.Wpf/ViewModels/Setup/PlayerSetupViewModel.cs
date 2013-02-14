namespace GuiWpf.ViewModels.Setup
{
    public class PlayerSetupViewModel : ViewModelBase
    {
        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { NotifyOfPropertyChange(value, () => IsEnabled, x => _isEnabled = x); }
        }
    }
}