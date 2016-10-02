using System;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class SelectSourceRegionForFortificationInteractionStateTests
    {
        private readonly SelectSourceRegionForFortificationInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectSourceRegionForFortificationObserver _selectSourceRegionForFortificationObserver;

        public SelectSourceRegionForFortificationInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectSourceRegionForFortificationObserver = Substitute.For<ISelectSourceRegionForFortificationObserver>();

            _sut = new SelectSourceRegionForFortificationInteractionState(_selectSourceRegionForFortificationObserver);
        }

        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _attackPhase.RegionsThatCanBeSourceForAttackOrFortification.Returns(new[] { region });

            _sut.AssertOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();

            _sut.AssertOnClickThrows<InvalidOperationException>(region);
        }
    }
}