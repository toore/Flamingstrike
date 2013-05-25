using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace RISK.Tests.Views
{
    [TestFixture]
    public class ConfirmViewModelFactoryTests
    {
        private IScreenService _screenService;
        private IResourceManagerWrapper _resourceManagerWrapper;

        [SetUp]
        public void SetUp()
        {
            _screenService = Substitute.For<IScreenService>();
            _resourceManagerWrapper = Substitute.For<IResourceManagerWrapper>();
        }

        [Test]
        public void Initialize_message()
        {
            Create("message").Message.Should().Be("message");
        }

        [Test]
        public void Initialize_default_confirm_and_abort_texts()
        {
            _resourceManagerWrapper.GetString("CANCEL").Returns("translated cancel text");

            var confirmViewModel = Create(null);

            confirmViewModel.ConfirmText.Should().Be("OK");
            confirmViewModel.AbortText.Should().Be("translated cancel text");
        }

        [Test]
        public void Initialize_confirm_and_abort_texts()
        {
            var confirmViewModel = Create(null, "yes", "no");

            confirmViewModel.ConfirmText.Should().Be("yes");
            confirmViewModel.AbortText.Should().Be("no");
        }

        [Test]
        public void Confirm_closes()
        {
            var confirmViewModel = Create("message");

            confirmViewModel.Confirm();

            _screenService.Received(1).Confirm(confirmViewModel);
        }

        [Test]
        public void Cancel_closes()
        {
            var confirmViewModel = Create("message");

            confirmViewModel.Cancel();

            _screenService.Received(1).Cancel(confirmViewModel);
        }

        public ConfirmViewModel Create(string message, string confirmText = null, string abortText = null)
        {
            var factory = new ConfirmViewModelFactory(_screenService, _resourceManagerWrapper);

            return factory.Create(message, confirmText, abortText);
        }
    }
}