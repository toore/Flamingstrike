using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace FlamingStrike.UI.WPF.Views.Preparation
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

            PotentialPlayers = new List<GamePreparationPlayerViewModel>(new[]
                {
                    new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 1", Color = Colors.DeepPink },
                    new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 2", Color = Colors.YellowGreen },
                    new GamePreparationPlayerViewModel(playerTypes) { Name = "Player 3", Color = Colors.Teal }
                });
        }

        public IList<GamePreparationPlayerViewModel> PotentialPlayers { get; }

        public Task ConfirmAsync()
        {
            throw new NotImplementedException();
        }
    }
}