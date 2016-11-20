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
        //public string Message => "Message displays here";
        //public string ConfirmText => "Confirm";
        //public string AbortText => "Abort";
        public ConfirmViewModelDesignerData()
            : base(null)
        {
            Message = "Message displays here";
            ConfirmText = "Confirm";
            AbortText = "Abort";
        }
    }
}