using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using Xunit;

namespace RISK.Tests.Views
{
    public class ConfirmViewModelFactoryTests
    {
        private readonly IScreenService _screenService;
        private readonly ILanguageResources _languageResources;

        public ConfirmViewModelFactoryTests()
        {
            _screenService = Substitute.For<IScreenService>();
            _languageResources = Substitute.For<ILanguageResources>();

            LanguageResources.Instance = _languageResources;
        }

        [Fact]
        public void Initialize_message()
        {
            Create("message").Message.Should().Be("message");
        }

        [Fact]
        public void Initialize_default_confirm_and_abort_texts()
        {
            _languageResources.GetString("CANCEL").Returns("translated cancel text");

            var confirmViewModel = Create(null);

            confirmViewModel.ConfirmText.Should().Be("OK");
            confirmViewModel.AbortText.Should().Be("translated cancel text");
        }

        [Fact]
        public void Initialize_confirm_and_abort_texts()
        {
            var confirmViewModel = Create(null, null,  "yes", "no");

            confirmViewModel.ConfirmText.Should().Be("yes");
            confirmViewModel.AbortText.Should().Be("no");
        }

        [Fact]
        public void Confirm_closes()
        {
            var confirmViewModel = Create("message");

            confirmViewModel.Confirm();

            _screenService.Received(1).Confirm(confirmViewModel);
        }

        [Fact]
        public void Cancel_closes()
        {
            var confirmViewModel = Create("message");

            confirmViewModel.Cancel();

            _screenService.Received(1).Cancel(confirmViewModel);
        }

        [Fact]
        public void Sets_display_name()
        {
            var confirmViewModel = Create(null, "display name");

            confirmViewModel.DisplayName.Should().Be("display name");
        }

        public ConfirmViewModel Create(string message, string displayName = null, string confirmText = null, string abortText = null)
        {
            var factory = new ConfirmViewModelFactory(_screenService);

            return factory.Create(message, displayName, confirmText, abortText);
        }
    }
}