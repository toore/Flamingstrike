using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public partial class GameOverView
    {
        public GameOverView()
        {
            InitializeComponent();
        }
    }

    public class GameOverViewModelDesignerData : GameOverViewModel
    {
        public GameOverViewModelDesignerData() : base("Fake player") { }
    }
}