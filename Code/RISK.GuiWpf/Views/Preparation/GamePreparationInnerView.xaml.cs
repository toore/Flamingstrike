using System.Collections.ObjectModel;
using System.Windows;
using GuiWpf.ViewModels.Preparation;

namespace GuiWpf.Views.Preparation
{
    public partial class GamePreparationInnerView
    {
        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(ObservableCollection<GamePreparationPlayerViewModel>), typeof(GamePreparationInnerView), new PropertyMetadata(default(ObservableCollection<GamePreparationPlayerViewModel>)));

        public GamePreparationInnerView()
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