using GuiWpf.ViewModels.Gameplay;
using GuiWpf.Views.WorldMapViews;

namespace RISK.Tests
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = GameboardViewModelTestDataFactory.ViewModel;
        }
    }
}