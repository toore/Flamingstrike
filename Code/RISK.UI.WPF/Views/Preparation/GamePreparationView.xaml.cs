using System;
using System.Collections.ObjectModel;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.Views.Preparation
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

            Players = new ObservableCollection<GamePreparationPlayerViewModel>(new[]
            {
                new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 1" },
                new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 2" },
                new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 3" }
            });
        }

        public ObservableCollection<GamePreparationPlayerViewModel> Players { get; }

        public void Confirm()
        {
            throw new NotImplementedException();
        }
    }
}