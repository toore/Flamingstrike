using System.Threading;

namespace GuiWpf.ViewModels.Setup
{
    public class UserInteractionSynchronizer : IUserInteractionSynchronizer
    {
        private readonly AutoResetEvent _userInteractionRequest = new AutoResetEvent(false);
        private readonly AutoResetEvent _userIsDoneInteracting = new AutoResetEvent(false);

        public void WaitForUserInteractionRequest()
        {
            _userInteractionRequest.WaitOne();
        }

        public void RequestUserInteraction()
        {
            _userInteractionRequest.Set();
        }

        public void WaitForUserToBeDoneWithInteracting()
        {
            _userIsDoneInteracting.WaitOne();
        }

        public void UserIsDoneInteracting()
        {
            _userIsDoneInteracting.Set();
        }

    }
}