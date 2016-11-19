using NSubstitute;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectSourceRegionForFortificationInteractionStateTests
    {
        private readonly SelectSourceRegionForFortificationInteractionState _sut;
        private readonly ISelectSourceRegionForFortificationInteractionStateObserver _selectSourceRegionForFortificationInteractionStateObserver;

        public SelectSourceRegionForFortificationInteractionStateTests()
        {
            _selectSourceRegionForFortificationInteractionStateObserver = Substitute.For<ISelectSourceRegionForFortificationInteractionStateObserver>();

            _sut = new SelectSourceRegionForFortificationInteractionState(_selectSourceRegionForFortificationInteractionStateObserver);
        }

        [Fact]
        public void Selects_source_region_for_fortification()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _selectSourceRegionForFortificationInteractionStateObserver.Received().Select(region);
        }
    }
}