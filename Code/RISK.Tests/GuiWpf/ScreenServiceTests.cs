using Caliburn.Micro;
using GuiWpf.Services;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class ScreenServiceTests
    {
        private readonly ScreenService _screenService;

        public ScreenServiceTests()
        {
            _screenService = new ScreenService();
        }

        [Fact]
        public void Confirm_calls_try_close_with_dialog_result_true()
        {
            var screen = Substitute.For<Screen>();
            _screenService.Confirm(screen);

            screen.Received().TryClose(true);
        }

        [Fact]
        public void Cancle_calls_try_close_with_dialog_result_false()
        {
            var screen = Substitute.For<Screen>();
            _screenService.Cancel(screen);

            screen.Received().TryClose(false);
        }
    }
}