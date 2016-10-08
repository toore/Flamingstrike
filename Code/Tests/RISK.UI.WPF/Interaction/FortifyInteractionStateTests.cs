using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
{
    public class FortifyInteractionStateTests
    {
        private readonly IRegion _selectedRegion;
        private readonly IAttackPhase _attackPhase;
        private readonly IDeselectRegionToFortifyFromObserver _deselectRegionToFortifyFromObserver;
        private readonly FortifyInteractionState _sut;

        public FortifyInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectedRegion = Substitute.For<IRegion>();
            _deselectRegionToFortifyFromObserver = Substitute.For<IDeselectRegionToFortifyFromObserver>();

            _sut = new FortifyInteractionState(_attackPhase, _selectedRegion, _deselectRegionToFortifyFromObserver);
        }

        [Fact]
        public void Fortifies_territory()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _attackPhase.Received().Fortify(_selectedRegion, region, 1);
        }

        [Fact]
        public void Deselects_region()
        {
            var region = Substitute.For<IRegion>();

            _sut.OnClick(_selectedRegion);

            _deselectRegionToFortifyFromObserver.Received().DeselectRegion();
        }
    }
}