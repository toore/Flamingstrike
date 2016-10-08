using NSubstitute;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectSourceRegionForFortificationInteractionStateTests
    {
        private readonly SelectSourceRegionForFortificationInteractionState _sut;
        private readonly ISelectSourceRegionForFortificationObserver _selectSourceRegionForFortificationObserver;

        public SelectSourceRegionForFortificationInteractionStateTests()
        {
            _selectSourceRegionForFortificationObserver = Substitute.For<ISelectSourceRegionForFortificationObserver>();

            _sut = new SelectSourceRegionForFortificationInteractionState(_selectSourceRegionForFortificationObserver);
        }

        [Fact]
        public void Selects_a_region_to_use_as_a_source_for_fortification()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _selectSourceRegionForFortificationObserver.Received().SelectSourceRegion(region);
        }
    }
}