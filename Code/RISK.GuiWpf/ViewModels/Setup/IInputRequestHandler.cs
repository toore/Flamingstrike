namespace GuiWpf.ViewModels.Setup
{
    public interface IInputRequestHandler
    {
        void WaitForInputRequest();
        void RequestInput();
        void WaitForInputAvailable();
        void InputIsAvailable();
    }
}