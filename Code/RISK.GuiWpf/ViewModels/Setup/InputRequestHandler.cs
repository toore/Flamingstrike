using System.Threading;

namespace GuiWpf.ViewModels.Setup
{
    public class InputRequestHandler : IInputRequestHandler
    {
        private readonly AutoResetEvent _inputRequest = new AutoResetEvent(false);
        private readonly AutoResetEvent _inputResultAvailable = new AutoResetEvent(false);

        public void WaitForInputRequest()
        {
            _inputRequest.WaitOne();
        }

        public void RequestInput()
        {
            _inputRequest.Set();
        }

        public void WaitForInputAvailable()
        {
            _inputResultAvailable.WaitOne();
        }

        public void InputIsAvailable()
        {
            _inputResultAvailable.Set();
        }

    }
}