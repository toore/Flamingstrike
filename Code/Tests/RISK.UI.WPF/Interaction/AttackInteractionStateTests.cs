using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class AttackInteractionStateTests
    {
        private readonly AttackInteractionState _sut;
        private readonly IAttackPhase _attackPhase;
        private readonly IRegion _selectedRegion;
        private readonly IRegion _attackedRegion;
        private readonly IDeselectAttackingRegionObserver _deselectAttackingRegionObserver;

        public AttackInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectedRegion = Substitute.For<IRegion>();
            _deselectAttackingRegionObserver = Substitute.For<IDeselectAttackingRegionObserver>();

            _sut = new AttackInteractionState(_attackPhase, _selectedRegion, _deselectAttackingRegionObserver);

            _attackedRegion = Substitute.For<IRegion>();
        }

        [Fact]
        public void OnClick_attacks()
        {
            _sut.OnClick(_attackedRegion);

            _attackPhase.Received().Attack(_selectedRegion, _attackedRegion);
        }

        [Fact]
        public void OnClick_deselects()
        {
            _sut.OnClick(_selectedRegion);

            _deselectAttackingRegionObserver.Received().DeselectRegion();
        }
    }
}