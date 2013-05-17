using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace RISK.Tests.Views
{
    [TestFixture]
    public class UserNotifierTests
    {
        private UserNotifier _userNotifier;
        private IWindowManager _windowManager;
        private IConfirmViewModelFactory _confirmViewModelFactory;

        [SetUp]
        public void SetUp()
        {
            _windowManager = Substitute.For<IWindowManager>();
            _confirmViewModelFactory = Substitute.For<IConfirmViewModelFactory>();

            _userNotifier = new UserNotifier(_windowManager, _confirmViewModelFactory);
        }

        [Test]
        public void Confirms_dialog()
        {
            var confirmViewModel = new ConfirmViewModel();
            _confirmViewModelFactory.Create().Returns(confirmViewModel);
            _windowManager.ShowDialog(confirmViewModel).Returns(true);

            var confirm = _userNotifier.Confirm("message");

            _windowManager.Received(1).ShowDialog(confirmViewModel);
            confirm.Should().BeTrue();
        }
    }
}