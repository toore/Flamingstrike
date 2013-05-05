using System.Threading.Tasks;
using GuiWpf.ViewModels.Setup;
using NUnit.Framework;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class UserInputRequestHandlerTests
    {
        private InputRequestHandler _inputRequestHandler;

        [SetUp]
        public void SetUp()
        {
            _inputRequestHandler = new InputRequestHandler();
        }

        [Test]
        public void Waits_for_input()
        {
            Task.Run(() => _inputRequestHandler.WaitForInputAvailable());

            _inputRequestHandler.InputIsAvailable();
        }

        [Test]
        public void Does_not_wait_for_input_when_input_already_available()
        {
            _inputRequestHandler.InputIsAvailable();

            _inputRequestHandler.WaitForInputAvailable();
        }
    }
}