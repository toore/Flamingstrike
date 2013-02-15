using System.Collections.ObjectModel;
using System.Windows;
using GuiWpf.ViewModels.Setup;

namespace GuiWpf.Views.GameSetupViews
{
    public partial class GameSetupView
    {
        public static readonly DependencyProperty PlayersProperty = DependencyProperty.Register("Players", typeof(ObservableCollection<PlayerSetupViewModel>), typeof(GameSetupView), new PropertyMetadata(default(ObservableCollection<PlayerSetupViewModel>)));

        public GameSetupView()
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