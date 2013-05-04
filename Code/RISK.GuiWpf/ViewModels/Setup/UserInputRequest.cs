using System.Threading;

namespace GuiWpf.ViewModels.Setup
{
    public class UserInputRequest : IUserInputRequest
    {
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public void WaitForInput()
        {
            _autoResetEvent.WaitOne();
        }

        public void InputHandled()
        {
            _autoResetEvent.Set();
        }
    }
}