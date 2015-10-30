using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf.Interaction
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritoryId _selectedTerritoryId;

        public AttackStateTests()
        {
            _selectedTerritoryId = Make.TerritoryId.Build();

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedTerritoryId = _selectedTerritoryId;
        }

        [Fact]
        public void Can_click_attackee_candidate()
        {
            var attackedTerritory = Substitute.For<ITerritoryId>();
            //_game.GetAttackeeCandidates(_selectedTerritory).Returns(new[] { attackeeCandidate });
            _game.CanAttack(_selectedTerritoryId, attackedTerritory).Returns(true);

            _sut.CanClick(attackedTerritory).Should().BeTrue();
        }

        [Fact]
        public void Click_on_attackee_candidate_attacks()
        {
            var attackeeCandidate = Substitute.For<ITerritoryId>();
            _game.CanAttack(_selectedTerritoryId, attackeeCandidate).Returns(true);

            _sut.OnClick(attackeeCandidate);

            _game.Received().Attack(_selectedTerritoryId, attackeeCandidate);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritoryId>());
        }

        [Fact]
        public void Click_on_remote_territory_should_throw()
        {
            var remoteTerritory = Substitute.For<ITerritoryId>();
            Action act = () => _sut.OnClick(remoteTerritory);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClick(_selectedTerritoryId);
        }

        [Fact]
        public void Click_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState().Returns(selectState);

            _sut.OnClick(_selectedTerritoryId);

            _sut.CurrentState.Should().Be(selectState);
            _sut.SelectedTerritoryId.Should().BeNull();
        }

        [Fact]
        public void When_entering_select_state_the_selected_territory_is_reset()
        {
            _sut.OnClick(_selectedTerritoryId);

            _sut.SelectedTerritoryId.Should().BeNull();
        }
    }
}