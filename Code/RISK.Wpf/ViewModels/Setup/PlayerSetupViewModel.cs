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

        private string _name;
        public string Name
        {
            get { return _name; }
            set { NotifyOfPropertyChange(value, () => Name, s => _name = s); }
        }

        private PlayerSetupType _playerSetupType;
        public PlayerSetupType PlayerSetupType
        {
            get { return _playerSetupType; }
            set { NotifyOfPropertyChange(value, () => PlayerSetupType, type => _playerSetupType = type); }
        }
    }

    public enum PlayerSetupType
    {
        Human,
        ComputerAdvanced,
        ComputerNeutral
    }
}