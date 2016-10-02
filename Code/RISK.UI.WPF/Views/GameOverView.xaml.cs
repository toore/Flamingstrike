using RISK.UI.WPF.ViewModels;

namespace RISK.UI.WPF.Views
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