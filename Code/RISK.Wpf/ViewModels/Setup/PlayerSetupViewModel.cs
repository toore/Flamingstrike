using System.Collections.Generic;

namespace GuiWpf.ViewModels.Setup
{
    public class PlayerSetupViewModel : ViewModelBase
    {
        private readonly IPlayerTypesFactory _playerTypesFactory;

        public PlayerSetupViewModel(IPlayerTypesFactory playerTypesFactory)
        {
            _playerTypesFactory = playerTypesFactory;
        }

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

        private PlayerTypeBase _selectedPlayerType;
        public PlayerTypeBase SelectedPlayerType
        {
            get { return _selectedPlayerType; }
            set { NotifyOfPropertyChange(value, () => SelectedPlayerType, type => _selectedPlayerType = type); }
        }

        public IEnumerable<PlayerTypeBase> PlayerTypes
        {
            get { return _playerTypesFactory.Create(); }
        }
    }
}