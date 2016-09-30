using FluentAssertions;
using GuiWpf.Properties;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class ConfirmViewModelFactoryTests
    {
        private readonly IScreenConfirmationService _screenConfirmationService;

        public ConfirmViewModelFactoryTests()
        {
            _screenConfirmationService = Substitute.For<IScreenConfirmationService>();
        }

        [Fact]
        public void Initialize_message()
        {
            Create("message").Message.Should().Be("message");
        }

        [Fact]
        public void Initialize_default_confirm_and_abort_texts()
        {
            var sut = Create(null);

            sut.ConfirmText.Should().Be("OK");
            sut.AbortText.Should().Be(Resources.CANCEL);
        }

        [Fact]
        public void Initialize_confirm_and_abort_texts()
        {
            var sut = Create(null, null, "yes", "no");

            sut.ConfirmText.Should().Be("yes");
            sut.AbortText.Should().Be("no");
        }

        [Fact]
        public void Confirm_closes()
        {
            var sut = Create("message");

            sut.Confirm();

            _screenConfirmationService.Received(1).Confirm(sut);
        }

        [Fact]
        public void Cancel_closes()
        {
            var sut = Create("message");

            sut.Cancel();

            _screenConfirmationService.Received(1).Cancel(sut);
        }

        [Fact]
        public void Sets_display_name()
        {
            var sut = Create(null, "display name");

            sut.DisplayName.Should().Be("display name");
        }

        private ConfirmViewModel Create(string message, string displayName = null, string confirmText = null, string abortText = null)
        {
            var factory = new ConfirmViewModelFactory(_screenConfirmationService);

            return factory.Create(message, displayName, confirmText, abortText);
        }
    }
}