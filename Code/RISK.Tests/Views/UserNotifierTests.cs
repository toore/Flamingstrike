using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace RISK.Tests.Views
{
    [TestFixture]
    public class UserNotifierTests
    {
        private UserNotifier _userNotifier;
        private IWindowManager _windowManager;
        private IConfirmViewModelFactory _confirmViewModelFactory;

        [SetUp]
        public void SetUp()
        {
            _windowManager = Substitute.For<IWindowManager>();
            _confirmViewModelFactory = Substitute.For<IConfirmViewModelFactory>();

            _userNotifier = new UserNotifier(_windowManager, _confirmViewModelFactory);
        }

        [Test]
        [TestCase(true, TestName = "Confirms")]
        [TestCase(false, TestName = "Cancels")]
        public void Shows_confirm_dialog(bool expected)
        {
            var confirmViewModel = new ConfirmViewModel(null, null);
            _confirmViewModelFactory.Create("message").Returns(confirmViewModel);
            _windowManager.ShowDialog(confirmViewModel).Returns(expected);

            var confirm = _userNotifier.Confirm("message");

            _windowManager.Received(1).ShowDialog(confirmViewModel);
            confirm.Should().Be(expected);
        }
    }
}