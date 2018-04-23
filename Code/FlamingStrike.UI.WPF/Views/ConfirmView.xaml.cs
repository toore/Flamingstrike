using FlamingStrike.UI.WPF.ViewModels;

namespace FlamingStrike.UI.WPF.Views
{
    public partial class ConfirmView
    {
        public ConfirmView()
        {
            InitializeComponent();
        }
    }

    public class ConfirmViewModelDesignerData : ConfirmViewModel
    {
        public ConfirmViewModelDesignerData()
            : base(null)
        {
            Message = "Message displays here";
            ConfirmText = "Confirm";
            AbortText = "Abort";
        }
    }
}