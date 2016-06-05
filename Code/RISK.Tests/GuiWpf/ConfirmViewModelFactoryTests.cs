using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class ConfirmViewModelFactoryTests
    {
        private readonly IScreenConfirmationService _screenConfirmationService;
        private readonly IResourceManager _resourceManager;

        public ConfirmViewModelFactoryTests()
        {
            _screenConfirmationService = Substitute.For<IScreenConfirmationService>();
            _resourceManager = Substitute.For<IResourceManager>();

            ResourceManager.Instance = _resourceManager;
        }

        [Fact]
        public void Initialize_message()
        {
            Create("message").Message.Should().Be("message");
        }

        [Fact]
        public void Initialize_default_confirm_and_abort_texts()
        {
            _resourceManager.GetString("CANCEL").Returns("translated cancel text");

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

            _screenConfirmationService.Received(1).Confirm(confirmViewModel);
        }

        [Fact]
        public void Cancel_closes()
        {
            var confirmViewModel = Create("message");

            confirmViewModel.Cancel();

            _screenConfirmationService.Received(1).Cancel(confirmViewModel);
        }

        [Fact]
        public void Sets_display_name()
        {
            var confirmViewModel = Create(null, "display name");

            confirmViewModel.DisplayName.Should().Be("display name");
        }

        public ConfirmViewModel Create(string message, string displayName = null, string confirmText = null, string abortText = null)
        {
            var factory = new ConfirmViewModelFactory(_screenConfirmationService);

            return factory.Create(message, displayName, confirmText, abortText);
        }
    }
}