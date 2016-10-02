using System;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class FortifyInteractionStateTests
    {
        private readonly IRegion _selectedRegion;
        private readonly IRegion _regionToFortify;
        private readonly IGame _game;
        private readonly FortifyInteractionState _sut;

        public FortifyInteractionStateTests()
        {
            _sut = new FortifyInteractionState(null, _selectedRegion, null);

            _regionToFortify = Substitute.For<IRegion>();
        }

        [Fact]
        public void Can_click_territory_that_can_be_fortified()
        {
            //_game.CanFortify(_selectedRegion, _regionToFortify).Returns(true);

            _sut.AssertOnClickCanBeInvoked(_regionToFortify);
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertOnClickCanBeInvoked(_selectedRegion);
        }

        [Fact]
        public void Can_not_click_when_fortification_is_not_allowed()
        {
            _sut.AssertOnClickThrows<InvalidOperationException>(_regionToFortify);
        }

        [Fact]
        public void OnClick_fortifies()
        {
            //_game.CanFortify(_selectedRegion, _regionToFortify).Returns(true);

            _sut.OnClick(_regionToFortify);

            //_game.Received().Fortify(_selectedRegion, _regionToFortify, 1);
            throw new NotImplementedException();
        }
    }
}