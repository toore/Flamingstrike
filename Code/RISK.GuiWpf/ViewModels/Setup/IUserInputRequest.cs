namespace GuiWpf.ViewModels.Setup
{
    public interface IUserInputRequest
    {
        void WaitForInput();
        void InputHandled();
    }
}