using System;
using System.Collections.Generic;
using System.Windows.Media;
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

            Players = new List<GamePreparationPlayerViewModel>(new[]
                {
                    new GamePreparationPlayerViewModel(playerTypes, new PlayerColor(Colors.Black)) { Name = "Player 1" },
                    new GamePreparationPlayerViewModel(playerTypes, new PlayerColor(Colors.Red)) { Name = "Player 2" },
                    new GamePreparationPlayerViewModel(playerTypes, new PlayerColor(Colors.CadetBlue)) { Name = "Player 3" }
                });
        }

        public IList<GamePreparationPlayerViewModel> Players { get; }

        public void Confirm()
        {
            throw new NotImplementedException();
        }
    }
}