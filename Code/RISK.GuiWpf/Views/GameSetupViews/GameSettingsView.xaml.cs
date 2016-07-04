using System;
using System.Collections.ObjectModel;

namespace GuiWpf.ViewModels.Settings
{
    public partial class GameSettingsView
    {
        public GameSettingsView()
        {
            InitializeComponent();
        }
    }

    public class GameSettingsViewDesignerData : IGameSettingsViewModel
    {
        public GameSettingsViewDesignerData()
        {
            var playerTypes = new PlayerTypes();

            Players = new ObservableCollection<PlayerSetupViewModel>(new[]
            {
                new PlayerSetupViewModel(playerTypes) { Name = "Player 1" },
                new PlayerSetupViewModel(playerTypes) { Name = "Player 2" },
                new PlayerSetupViewModel(playerTypes) { Name = "Player 3" }
            });
        }

        public ObservableCollection<PlayerSetupViewModel> Players { get; }

        public void Confirm()
        {
            throw new NotImplementedException();
        }
    }
}