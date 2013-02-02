using GuiWpf.Views.WorldMapView;

namespace GuiWpf.Views.Main
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