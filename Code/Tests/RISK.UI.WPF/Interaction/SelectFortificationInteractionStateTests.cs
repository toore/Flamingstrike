using NSubstitute;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectFortificationInteractionStateTests
    {
        private readonly SelectFortificationInteractionState _sut;
        private readonly ISelectFortificationInteractionStateObserver _selectFortificationInteractionStateObserver;

        public SelectFortificationInteractionStateTests()
        {
            _selectFortificationInteractionStateObserver = Substitute.For<ISelectFortificationInteractionStateObserver>();

            _sut = new SelectFortificationInteractionState(_selectFortificationInteractionStateObserver);
        }

        [Fact]
        public void Selects_a_region_to_use_as_a_source_for_fortification()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _selectFortificationInteractionStateObserver.Received().Select(region);
        }
    }
}