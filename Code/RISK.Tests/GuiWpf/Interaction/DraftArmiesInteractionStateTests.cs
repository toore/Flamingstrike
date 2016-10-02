using System.Linq;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Play;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
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
        public void Can_click_region_occupied_by_current_player_and_with_armies_to_draft()
        {
            var region = Substitute.For<IRegion>();
            _draftArmiesPhase.RegionsAllowedToDraftArmies.Returns(new[] { region });
            _draftArmiesPhase.NumberOfArmiesToDraft.Returns(1);

            _sut.AssertOnClickCanBeInvoked(region);
        }

        [Fact]
        public void Can_not_click_region_not_occupied_by_current_player()
        {
            var region = Substitute.For<IRegion>();
            _draftArmiesPhase.NumberOfArmiesToDraft.Returns(1);

            _sut.AssertOnClickThrowsInvalidOperationException(region);
        }

        [Fact]
        public void Can_not_click_region_when_no_armies_to_draft()
        {
            var region = Substitute.For<IRegion>();
            _draftArmiesPhase.RegionsAllowedToDraftArmies.Returns(Enumerable.Empty<IRegion>());

            _sut.AssertOnClickThrowsInvalidOperationException(region);
        }

        [Fact]
        public void OnClick_places_draft_army()
        {
            var region = Substitute.For<IRegion>();
            _draftArmiesPhase.RegionsAllowedToDraftArmies.Returns(new[] { region });
            _draftArmiesPhase.NumberOfArmiesToDraft.Returns(1);

            _sut.OnClick(region);

            _draftArmiesPhase.Received().PlaceDraftArmies(region, 1);
        }
    }
}