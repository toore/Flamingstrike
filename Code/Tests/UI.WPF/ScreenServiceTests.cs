using System.Threading.Tasks;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.Services;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF
{
    public class ScreenServiceTests
    {
        private readonly ScreenConfirmationService _screenConfirmationService;

        public ScreenServiceTests()
        {
            _screenConfirmationService = new ScreenConfirmationService();
        }

        [Fact]
        public async Task Confirm_calls_try_close_with_dialog_result_true()
        {
            var screen = Substitute.For<Screen>();
            await _screenConfirmationService.Confirm(screen);

            await screen.Received().TryCloseAsync(true);
        }

        [Fact]
        public async Task Cancle_calls_try_close_with_dialog_result_false()
        {
            var screen = Substitute.For<Screen>();
            await _screenConfirmationService.Cancel(screen);

            await screen.Received().TryCloseAsync(false);
        }
    }
}