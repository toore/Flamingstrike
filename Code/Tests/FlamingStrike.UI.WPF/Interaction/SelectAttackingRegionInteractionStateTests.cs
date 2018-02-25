using FlamingStrike.GameEngine;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF.Interaction
{
    public class SelectAttackingRegionInteractionStateTests
    {
        private readonly SelectAttackingRegionInteractionState _sut;
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingRegionInteractionStateTests()
        {
            _selectAttackingRegionInteractionStateObserver = Substitute.For<ISelectAttackingRegionInteractionStateObserver>();

            _sut = new SelectAttackingRegionInteractionState(_selectAttackingRegionInteractionStateObserver);
        }

        [Fact]
        public void Selects_a_region_to_attack_from()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnRegionClicked(region);

            _selectAttackingRegionInteractionStateObserver.Received().Select(region);
        }
    }
}