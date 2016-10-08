using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.UI.WPF.ViewModels.Gameplay.Interaction;
using Xunit;

namespace Tests.RISK.UI.WPF.Interaction
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
            var region = Substitute.For<IRegion>();

            _sut.OnClick(region);

            _draftArmiesPhase.Received().PlaceDraftArmies(region, 1);
        }
    }
}