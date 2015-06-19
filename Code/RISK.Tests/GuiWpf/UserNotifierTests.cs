using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class UserNotifierTests
    {
        private UserNotifier _userNotifier;
        private IWindowManager _windowManager;
        private IConfirmViewModelFactory _confirmViewModelFactory;

        public UserNotifierTests()
        {
            _windowManager = Substitute.For<IWindowManager>();
            _confirmViewModelFactory = Substitute.For<IConfirmViewModelFactory>();

            _userNotifier = new UserNotifier(_windowManager, _confirmViewModelFactory);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Shows_confirm_dialog(bool expectedConfirmation)
        {
            var confirmViewModel = new ConfirmViewModel(null);
            _confirmViewModelFactory.Create("message", "display name", "confirm text", "abort text").Returns(confirmViewModel);
            _windowManager.ShowDialog(confirmViewModel).Returns(expectedConfirmation);

            var confirm = _userNotifier.Confirm("message", "display name", "confirm text", "abort text");

            _windowManager.Received(1).ShowDialog(confirmViewModel);
            confirm.Should().Be(expectedConfirmation);
        }
    }
}