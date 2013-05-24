using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using NUnit.Framework;
using NSubstitute;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class ScreenServiceTests
    {
        private ScreenService _screenService;

        [SetUp]
        public void SetUp()
        {
            _screenService = new ScreenService();
        }

        [Test]
        public void Confirm_calls_try_close_with_dialog_result_true()
        {
            var screen = Substitute.For<Screen>();
            _screenService.Confirm(screen);

            screen.Received().TryClose(true);
        }

        [Test]
        public void Cancle_calls_try_close_with_dialog_result_false()
        {
            var screen = Substitute.For<Screen>();
            _screenService.Cancel(screen);

            screen.Received().TryClose(false);
        }
    }
}