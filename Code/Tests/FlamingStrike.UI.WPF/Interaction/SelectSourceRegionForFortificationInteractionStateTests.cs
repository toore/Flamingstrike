using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF.Interaction
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