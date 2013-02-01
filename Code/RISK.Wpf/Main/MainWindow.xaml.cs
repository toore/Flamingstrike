using RISK.WorldMap;

namespace RISK.Main
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new WorldMapViewModelTestData();
        }
    }
}