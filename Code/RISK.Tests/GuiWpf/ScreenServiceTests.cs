using Caliburn.Micro;
using GuiWpf.Services;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class ScreenServiceTests
    {
        private readonly ScreenConfirmationService _screenConfirmationService;

        public ScreenServiceTests()
        {
            _screenConfirmationService = new ScreenConfirmationService();
        }

        [Fact]
        public void Confirm_calls_try_close_with_dialog_result_true()
        {
            var screen = Substitute.For<Screen>();
            _screenConfirmationService.Confirm(screen);

            screen.Received().TryClose(true);
        }

        [Fact]
        public void Cancle_calls_try_close_with_dialog_result_false()
        {
            var screen = Substitute.For<Screen>();
            _screenConfirmationService.Cancel(screen);

            screen.Received().TryClose(false);
        }
    }
}