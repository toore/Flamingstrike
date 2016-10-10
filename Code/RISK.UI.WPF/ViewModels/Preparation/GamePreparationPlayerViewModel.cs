using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.UI.WPF.ViewModels.Preparation
{
    public class GamePreparationPlayerViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private string _name;
        private PlayerTypeBase _selectedPlayerType;

        public GamePreparationPlayerViewModel(IPlayerTypes playerTypes)
        {
            PlayerTypes = playerTypes.Values;
            SelectedPlayerType = PlayerTypes.First();
        }

        public Action OnIsEnabledChanged = () => { };

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

        public string Name
        {
            get { return _name; }
            set { NotifyOfPropertyChange(value, () => Name, s => _name = s); }
        }

        public PlayerTypeBase SelectedPlayerType
        {
            get { return _selectedPlayerType; }
            set { NotifyOfPropertyChange(value, () => SelectedPlayerType, type => _selectedPlayerType = type); }
        }

        public IReadOnlyList<PlayerTypeBase> PlayerTypes { get; }
    }
}