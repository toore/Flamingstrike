using System.Threading.Tasks;
using GuiWpf.ViewModels.Setup;
using NUnit.Framework;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class UserInputRequestHandlerTests
    {
        private UserInputRequestHandler _userInputRequestHandler;

        [SetUp]
        public void SetUp()
        {
            _userInputRequestHandler = new UserInputRequestHandler();
        }

        [Test]
        public void Waits_for_input()
        {
            Task.Run(() => _userInputRequestHandler.WaitForInput());

            _userInputRequestHandler.InputHandled();
        }

        [Test]
        public void Does_not_wait_for_input_when_input_already_handled()
        {
            _userInputRequestHandler.InputHandled();

            _userInputRequestHandler.WaitForInput();
        }
    }
}