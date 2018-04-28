using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF.Interaction
{
    public class FortifyInteractionStateTests
    {
        private readonly Region _selectedRegion;
        private readonly IAttackPhase _attackPhase;
        private readonly IFortifyInteractionStateObserver _fortifyInteractionStateObserver;
        private readonly FortifyInteractionState _sut;

        public FortifyInteractionStateTests()
        {
            _attackPhase = Substitute.For<IAttackPhase>();
            _selectedRegion = Region.Brazil;
            _fortifyInteractionStateObserver = Substitute.For<IFortifyInteractionStateObserver>();

            _sut = new FortifyInteractionState(_attackPhase, _selectedRegion, _fortifyInteractionStateObserver);
        }

        [Fact]
        public void Fortifies_territory()
        {
            var region = Region.NorthAfrica;

            _sut.OnRegionClicked(region);

            _attackPhase.Received().Fortify(_selectedRegion, region, 1);
        }

        [Fact]
        public void Deselects_region()
        {
            _sut.OnRegionClicked(_selectedRegion);

            _fortifyInteractionStateObserver.Received().DeselectRegion();
        }
    }
}