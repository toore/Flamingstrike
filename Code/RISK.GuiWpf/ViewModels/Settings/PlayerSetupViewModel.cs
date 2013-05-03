using System;
using System.Collections.Generic;
using System.Linq;

namespace GuiWpf.ViewModels.Settings
{
    public class PlayerSetupViewModel : ViewModelBase
    {
        public PlayerSetupViewModel(IPlayerTypes playerTypes)
        {
            PlayerTypes = playerTypes.Values;
            SelectedPlayerType = PlayerTypes.First();
        }

        public Action OnIsEnabledChanged = () => { };

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                NotifyOfPropertyChange(value, () => IsEnabled, x =>
                    {
                        _isEnabled = x;
                        OnIsEnabledChanged();
                    });
            }
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

        public IEnumerable<PlayerTypeBase> PlayerTypes { get; private set; }
    }
}