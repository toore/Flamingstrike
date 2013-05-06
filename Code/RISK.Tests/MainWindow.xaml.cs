using GuiWpf.ViewModels.Gameplay;

namespace RISK.Tests
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = GameboardViewModelTestData.ViewModel;
        }
    }
}