using GuiWpf.ViewModels;

namespace GuiWpf.Views
{
    public class ConfirmViewModelTestDataFactory
    {
        public static ConfirmViewModel ViewModel
        {
            get { return new ConfirmViewModelTestDataFactory().Create(); }
        }

        public ConfirmViewModel Create()
        {
            return new ConfirmViewModel(null)
                {
                    Message = "Message displays here",
                    ConfirmText = "Confirm text",
                    AbortText = "Abort text"
                };
        }
    }
}