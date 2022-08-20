using System.Threading.Tasks;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF
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
        public async Task Shows_confirm_dialog(bool expectedConfirmation)
        {
            var confirmViewModel = new ConfirmViewModel(null);
            _confirmViewModelFactory.Create("message", "display name", "confirm text", "abort text").Returns(confirmViewModel);
            _windowManager.ShowDialogAsync(confirmViewModel).Returns(Task.FromResult<bool?>(expectedConfirmation));

            var confirm = await _userNotifier.ConfirmAsync("message", "display name", "confirm text", "abort text");

            await _windowManager.Received(1).ShowDialogAsync(confirmViewModel);
            confirm.Should().Be(expectedConfirmation);
        }

        [Fact]
        public async Task Shows_notifying_dialog()
        {
            await _userNotifier.NotifyAsync("message", "display name");

            await _windowManager.Received(1).ShowDialogAsync(Arg.Is<NotifyViewModel>(vm =>
                vm.Message == "message"
                &&
                vm.DisplayName == "display name"));
        }
    }
}