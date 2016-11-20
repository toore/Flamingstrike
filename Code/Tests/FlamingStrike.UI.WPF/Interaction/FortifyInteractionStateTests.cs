using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF.Interaction
{
    public class FortifyInteractionStateTests
    {
        private readonly IRegion _selectedRegion;
        private readonly IAttackPhase _attackPhase;
        private readonly IFortifyInteractionStateObserver _fortifyInteractionStateObserver;
        private readonly FortifyInteractionState _sut;

        public FortifyInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectedRegion = Substitute.For<IRegion>();
            _fortifyInteractionStateObserver = Substitute.For<IFortifyInteractionStateObserver>();

            _sut = new FortifyInteractionState(_attackPhase, _selectedRegion, _fortifyInteractionStateObserver);
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

            _fortifyInteractionStateObserver.Received().DeselectRegion();
        }
    }
}