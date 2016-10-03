using System.Collections.ObjectModel;
using System.Windows;
using RISK.UI.WPF.ViewModels.Preparation;

namespace RISK.UI.WPF.Views.Preparation
{
    public partial class GamePreparationControl
    {
        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(ObservableCollection<GamePreparationPlayerViewModel>), typeof(GamePreparationControl), new PropertyMetadata(default(ObservableCollection<GamePreparationPlayerViewModel>)));

        public GamePreparationControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<GamePreparationPlayerViewModel> Players
        {
            get { return (ObservableCollection<GamePreparationPlayerViewModel>)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }
    }
}