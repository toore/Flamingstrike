using GuiWpf.ViewModels.Setup;

namespace RISK.Tests.Application.Specifications
{
    public class InputRequestHandlerSpy : IInputRequestHandler
    {
        //private readonly UserInputRequestHandler _userInputRequestHandler;

        //public UserInputRequestHandlerSpy(UserInputRequestHandler userInputRequestHandler)
        //{
        //    _userInputRequestHandler = userInputRequestHandler;
        //}

        public bool IsWaitingForInput { get; private set; }

        //public void WaitOne()
        //{
        //    IsWaitingForInput = true;
        //    _userInputRequestHandler.WaitOne();
        //}

        //public void Set()
        //{
        //    _userInputRequestHandler.Set();
        //    IsWaitingForInput = false;
        //}
        public void WaitForInputRequest()
        {
            throw new System.NotImplementedException();
        }

        public void RequestInput()
        {
            throw new System.NotImplementedException();
        }

        public void WaitForInputAvailable()
        {
            throw new System.NotImplementedException();
        }

        public void InputIsAvailable()
        {
            throw new System.NotImplementedException();
        }
    }
}