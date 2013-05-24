using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace RISK.Tests.Views
{
    [TestFixture]
    public class ConfirmViewModelTests
    {
        private ConfirmViewModel _confirmViewModel;
        private IScreenService _screenService;

        [SetUp]
        public void SetUp()
        {
            _screenService = Substitute.For<IScreenService>();

            _confirmViewModel = new ConfirmViewModel(_screenService, "message");
        }

        [Test]
        public void Initialize_message()
        {
            _confirmViewModel.Message.Should().Be("message");
        }

        [Test]
        public void Confirm_closes()
        {
            _confirmViewModel.Confirm();

            _screenService.Received(1).Confirm(_confirmViewModel);
        }

        [Test]
        public void Cancel_closes()
        {
            _confirmViewModel.Cancel();

            _screenService.Received(1).Cancel(_confirmViewModel);
        }
    }
}