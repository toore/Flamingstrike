using RISK.UI.WPF.ViewModels;

namespace RISK.UI.WPF.Views
{
    public partial class ConfirmView
    {
        public ConfirmView()
        {
            InitializeComponent();
        }
    }

    public class ConfirmViewModelDesignerData : IConfirmViewModel
    {
        public string Message => "Message displays here";
        public string ConfirmText => "Confirm";
        public string AbortText => "Abort";
    }
}