using System;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Application.World;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.Application.Interaction
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritory _selectedTerritory;

        public AttackStateTests()
        {
            _selectedTerritory = Make.Territory.Build();

            _sut.CurrentState = new AttackState(_interactionStateFactory);
            _sut.SelectedTerritory = _selectedTerritory;
        }

        [Fact]
        public void Can_click_attackee_candidate()
        {
            var attackeeCandidate = Substitute.For<ITerritory>();
            _game.GetAttackeeCandidates(_selectedTerritory).Returns(new[] { attackeeCandidate });

            _sut.CanClick(attackeeCandidate).Should().BeTrue();
        }

        [Fact]
        public void Click_on_attackee_candidate_attacks()
        {
            var attackeeCandidate = Substitute.For<ITerritory>();
            _game.GetAttackeeCandidates(_selectedTerritory).Returns(new[] { attackeeCandidate });

            _sut.OnClick(attackeeCandidate);

            _game.Received().Attack(_selectedTerritory, attackeeCandidate);
        }

        [Fact]
        public void Can_not_click_on_remote_territory()
        {
            _sut.AssertCanNotClick(Substitute.For<ITerritory>());
        }

        [Fact]
        public void Click_on_remote_territory_should_throw()
        {
            var remoteTerritory = Substitute.For<ITerritory>();
            Action act = () => _sut.OnClick(remoteTerritory);

            act.ShouldThrow<InvalidOperationException>();
        }

        [Fact]
        public void Can_click_on_selected_territory()
        {
            _sut.AssertCanClick(_selectedTerritory);
        }

        [Fact]
        public void Click_on_selected_territory_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState().Returns(selectState);

            _sut.OnClick(_selectedTerritory);

            _sut.CurrentState.Should().Be(selectState);
            _sut.SelectedTerritory.Should().BeNull();
        }

        [Fact]
        public void When_entering_select_state_the_selected_territory_is_reset()
        {
            _sut.OnClick(_selectedTerritory);

            _sut.SelectedTerritory.Should().BeNull();
        }
    }
}