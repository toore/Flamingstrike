namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInputRequestHandler
    {
        void WaitForInput();
        void InputHandled();
    }
}