using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF.Interaction
{
    public class SelectAttackingRegionInteractionStateTests
    {
        private readonly SelectAttackingRegionInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly ISelectAttackingRegionInteractionStateObserver _selectAttackingRegionInteractionStateObserver;

        public SelectAttackingRegionInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectAttackingRegionInteractionStateObserver = Substitute.For<ISelectAttackingRegionInteractionStateObserver>();

            _sut = new SelectAttackingRegionInteractionState(_attackPhase, _selectAttackingRegionInteractionStateObserver);
        }

        [Fact]
        public void Selects_a_region_to_attack_from()
        {
            var region = Region.Brazil;

            _sut.OnRegionClicked(region);

            _selectAttackingRegionInteractionStateObserver.Received().Select(region);
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