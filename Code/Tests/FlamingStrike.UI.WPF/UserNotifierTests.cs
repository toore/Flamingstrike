using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF
{
    public class UserNotifierTests
    {
        private readonly UserNotifier _userNotifier;
        private readonly IWindowManager _windowManager;
        private readonly IConfirmViewModelFactory _confirmViewModelFactory;

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

        [Fact]
        public void Shows_notifying_dialog()
        {
            _userNotifier.Notify("message", "display name");

            _windowManager.Received(1).ShowDialog(Arg.Is<NotifyViewModel>(vm =>
                vm.Message == "message"
                &&
                vm.DisplayName == "display name"));
        }
    }
}