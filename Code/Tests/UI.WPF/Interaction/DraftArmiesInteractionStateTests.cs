using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF.Interaction
{
    public class DraftArmiesInteractionStateTests
    {
        private readonly IDraftArmiesPhase _draftArmiesPhase;
        private readonly DraftArmiesInteractionState _sut;

        public DraftArmiesInteractionStateTests()
        {
            _draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();

            _sut = new DraftArmiesInteractionState(_draftArmiesPhase);
        }

        [Fact]
        public void Places_one_army_in_region()
        {
            var region = Region.Alaska;

            _sut.OnRegionClicked(region);

            _draftArmiesPhase.Received().PlaceDraftArmies(region, 1);
        }
    }
}