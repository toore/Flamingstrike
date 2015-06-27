using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Interaction
{
    public class SelectStateTests : InteractionStateTestsBase
    {
        [Fact]
        public void Can_click_territory_occupied_by_current_player()
        {
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.AssertCanClick(territory);
        }

        [Fact]
        public void Can_not_click_territory_not_occupied_by_current_player()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritory>());
        }

        [Fact]
        public void Click_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState().Returns(expected);
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.CurrentState.Should().Be(expected);
        }

        [Fact]
        public void When_entering_attack_state_the_selected_territory_is_updated()
        {
            var territory = CreateTerritoryOccupiedByCurrentPlayer();

            _sut.OnClick(territory);

            _sut.SelectedTerritory.Should().Be(territory);
        }

        private ITerritory CreateTerritoryOccupiedByCurrentPlayer()
        {
            var territory = Substitute.For<ITerritory>();
            _game.IsCurrentPlayerOccupyingTerritory(territory).Returns(true);
            return territory;
        }
    }
}