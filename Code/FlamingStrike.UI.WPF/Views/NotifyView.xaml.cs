using FlamingStrike.UI.WPF.ViewModels;

namespace FlamingStrike.UI.WPF.Views
{
    public partial class NotifyView
    {
        public NotifyView()
        {
            InitializeComponent();
        }
    }

    public class NotifyViewModelDesignerData : NotifyViewModel
    {
        public NotifyViewModelDesignerData()
        {
            Message = "Message displays here";
        }
    }
}