using GuiWpf.ViewModels;

namespace GuiWpf.Views
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
        public string ConfirmText => "Confirm text";
        public string AbortText => "Abort text";
    }
}