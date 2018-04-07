using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF.Interaction
{
    public class SelectSourceRegionForFortificationInteractionStateTests
    {
        private readonly SelectSourceRegionForFortificationInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectSourceRegionForFortificationInteractionStateObserver _selectSourceRegionForFortificationInteractionStateObserver;

        public SelectSourceRegionForFortificationInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectSourceRegionForFortificationInteractionStateObserver = Substitute.For<ISelectSourceRegionForFortificationInteractionStateObserver>();

            _sut = new SelectSourceRegionForFortificationInteractionState(_attackPhase, _selectSourceRegionForFortificationInteractionStateObserver);
        }

        [Fact]
        public void Selects_source_region_for_fortification()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnRegionClicked(region);

            _selectSourceRegionForFortificationInteractionStateObserver.Received().Select(region);
        }

        [Fact]
        public void Can_end_turn()
        {
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Ends_turn_when_invoked()
        {
            _sut.EndTurn();

            _attackPhase.Received().EndTurn();
        }
    }
}