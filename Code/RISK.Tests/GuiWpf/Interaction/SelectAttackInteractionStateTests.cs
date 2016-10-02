using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class SelectAttackInteractionStateTests
    {
        private readonly SelectAttackingRegionInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectAttackingRegionObserver _selectAttackingRegionObserver;

        public SelectAttackInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectAttackingRegionObserver = Substitute.For<ISelectAttackingRegionObserver>();

            _sut = new SelectAttackingRegionInteractionState(_selectAttackingRegionObserver);
        }

        [Fact]
        public void Can_click_region_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _attackPhase.RegionsThatCanBeSourceForAttackOrFortification.Returns(new[] { region });

            _sut.AssertOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_region_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            var anotherRegion = Make.Region.Build();
            _attackPhase.RegionsThatCanBeSourceForAttackOrFortification.Returns(new[] { anotherRegion });

            _sut.AssertOnClickThrowsInvalidOperationException(region);
        }
    }
}