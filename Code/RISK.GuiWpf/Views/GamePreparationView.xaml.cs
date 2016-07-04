using System;
using System.Collections.ObjectModel;
using GuiWpf.ViewModels.Settings;

namespace GuiWpf.Views
{
    public partial class GamePreparationView
    {
        public GamePreparationView()
        {
            InitializeComponent();
        }
    }

    public class GamePreparationViewDesignerData : IGamePreparationViewModel
    {
        public GamePreparationViewDesignerData()
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