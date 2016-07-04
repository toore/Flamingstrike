using System.Collections.ObjectModel;
using System.Windows;
using GuiWpf.ViewModels.Settings;

namespace GuiWpf.Views.GameSetupViews
{
    public partial class GamePreparationInnerView
    {
        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(ObservableCollection<PlayerSetupViewModel>), typeof(GamePreparationInnerView), new PropertyMetadata(default(ObservableCollection<PlayerSetupViewModel>)));

        public GamePreparationInnerView()
        {
            InitializeComponent();
        }

        public ObservableCollection<PlayerSetupViewModel> Players
        {
            get { return (ObservableCollection<PlayerSetupViewModel>)GetValue(PlayersProperty); }
            set { SetValue(PlayersProperty, value); }
        }
    }
}