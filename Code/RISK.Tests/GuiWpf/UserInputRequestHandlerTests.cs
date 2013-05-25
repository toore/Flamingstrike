using System.Threading.Tasks;
using GuiWpf.ViewModels.Setup;
using NUnit.Framework;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class UserInputRequestHandlerTests
    {
        private UserInteractionSynchronizer _userInteractionSynchronizer;

        [SetUp]
        public void SetUp()
        {
            _userInteractionSynchronizer = new UserInteractionSynchronizer();
        }

        [Test]
        public void Waits_for_input()
        {
            Task.Run(() => _userInteractionSynchronizer.WaitForUserToBeDoneWithInteracting());

            _userInteractionSynchronizer.UserIsDoneInteracting();
        }

        [Test]
        public void Does_not_wait_for_input_when_input_already_available()
        {
            _userInteractionSynchronizer.UserIsDoneInteracting();

            _userInteractionSynchronizer.WaitForUserToBeDoneWithInteracting();
        }
    }
}