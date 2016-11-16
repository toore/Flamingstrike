using NSubstitute;
using RISK.GameEngine;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectAttackingInteractionStateTests
    {
        private readonly SelectAttackingRegionInteractionState _sut;
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingInteractionStateTests()
        {
            _selectAttackingRegionInteractionStateObserver = Substitute.For<ISelectAttackingRegionInteractionStateObserver>();

            _sut = new SelectAttackingRegionInteractionState(_selectAttackingRegionInteractionStateObserver);
        }

        [Fact]
        public void Selects_a_region_to_attack_from()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _selectAttackingRegionInteractionStateObserver.Received().Select(region);
        }
    }
}