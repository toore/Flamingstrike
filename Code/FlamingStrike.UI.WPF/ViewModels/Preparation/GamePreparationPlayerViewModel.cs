using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace FlamingStrike.UI.WPF.ViewModels.Preparation
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
        private Color _color;

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

        public Color Color
        {
            get { return _color; }
            set { NotifyOfPropertyChange(value, () => Color, c => _color = c); }
        }
    }
}