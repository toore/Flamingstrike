using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class AttackStateTests : InteractionStateTestsBase
    {
        private readonly ITerritory _selectedTerritory;
        private readonly ILocation _selectedLocation;
        private readonly ILocation _borderingLocationOccupiedByOtherPlayer;
        private readonly Location _remoteLocationOccupiedByOtherPlayer;
        private readonly Location _borderingLocationOccupiedByPlayer;
        private readonly Territory _borderingTerritoryOccupiedByOtherPlayer;
        private readonly Location _remoteLocationOccupiedByPlayer;
        private Territory _borderingTerritoryOccupiedByPlayer;

        public AttackStateTests()
        {
            _borderingLocationOccupiedByPlayer = Make.Location.Build();
            _borderingTerritoryOccupiedByPlayer = Make.Territory
                .Location(_borderingLocationOccupiedByPlayer)
                .Occupant(Player)
                .Build();
            WorldMap.GetTerritory(_borderingLocationOccupiedByPlayer).Returns(_borderingTerritoryOccupiedByPlayer);

            _remoteLocationOccupiedByPlayer = Make.Location.Build();
            var remoteTerritoryOccupiedByPlayer = Make.Territory
                .Location(_remoteLocationOccupiedByPlayer)
                .Occupant(Player)
                .Build();
            WorldMap.GetTerritory(_remoteLocationOccupiedByPlayer).Returns(remoteTerritoryOccupiedByPlayer);

            var otherPlayer = Substitute.For<IPlayer>();
            
            _borderingLocationOccupiedByOtherPlayer = Make.Location.Build();
            _borderingTerritoryOccupiedByOtherPlayer = Make.Territory
                .Location(_borderingLocationOccupiedByOtherPlayer)
                .Occupant(otherPlayer)
                .Build();
            WorldMap.GetTerritory(_borderingLocationOccupiedByOtherPlayer).Returns(_borderingTerritoryOccupiedByOtherPlayer);

            _remoteLocationOccupiedByOtherPlayer = Make.Location.Build();
            var remoteTerritoryOccupiedByOtherPlayer = Make.Territory
                .Location(_remoteLocationOccupiedByOtherPlayer)
                .Occupant(otherPlayer)
                .Build();
            WorldMap.GetTerritory(_remoteLocationOccupiedByOtherPlayer).Returns(remoteTerritoryOccupiedByOtherPlayer);

            _selectedLocation = Make.Location
                .WithBorder(_borderingLocationOccupiedByOtherPlayer)
                .WithBorder(_borderingLocationOccupiedByPlayer)
                .Build();
            _selectedTerritory = Make.Territory
                .Location(_selectedLocation)
                .Occupant(Player)
                .Build();
            
            Sut = new AttackState(StateController, InteractionStateFactory, BattleCalculator, CardFactory, Player, WorldMap, _selectedTerritory);
        }

        [Fact]
        public void Can_select_location_selected()
        {
            Sut.CanSelect(_selectedLocation).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_not_selected_location()
        {
            Sut.CanSelect(Substitute.For<ILocation>()).Should().BeFalse();
            Sut.CanSelect(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Selecting_selected_location_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            InteractionStateFactory.CreateSelectState(Player, WorldMap).Returns(selectState);

            Sut.Select(_selectedLocation);

            StateController.CurrentState.Should().Be(selectState);
        }

        [Fact]
        public void Can_not_attack_bordering_location_when_having_one_army()
        {
            _selectedTerritory.Armies = 1;

            Sut.CanAttack(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_non_bordering_location()
        {
            _selectedTerritory.Armies = 2;

            Sut.CanAttack(_remoteLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_bordering_location_already_occupied()
        {
            _selectedTerritory.Armies = 2;

            Sut.CanAttack(_borderingLocationOccupiedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_attack_bordering_location_when_having_two_armies()
        {
            _selectedTerritory.Armies = 2;

            Sut.CanAttack(_borderingLocationOccupiedByOtherPlayer).Should().BeTrue();
        }

        [Fact]
        public void Attacks_when_territories_have_borders()
        {
            _selectedTerritory.Armies = 2;

            Sut.Attack(_borderingLocationOccupiedByOtherPlayer);

            BattleCalculator.Received().Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void After_successfull_attack_attack_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            InteractionStateFactory.CreateAttackState(Player, WorldMap, _borderingTerritoryOccupiedByOtherPlayer).Returns(expected);
            _selectedTerritory.Armies = 2;
            AttackerWins();

            Sut.Attack(_borderingLocationOccupiedByOtherPlayer);

            StateController.CurrentState.Should().Be(expected);
        }

        private void AttackerWins()
        {
            BattleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);
        }

        [Fact]
        public void Player_will_be_awarded_a_card_when_turn_ends()
        {
            _selectedTerritory.Armies = 2;
            BattleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);

            Sut.Attack(_borderingLocationOccupiedByOtherPlayer);
            Sut.EndTurn();

            StateController.PlayerShouldReceiveCardWhenTurnEnds.Should().BeTrue();
        }

        [Fact]
        public void Can_not_fortify()
        {
            Sut.CanFortify(_selectedLocation).Should().BeFalse();
            Sut.CanFortify(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
            Sut.CanFortify(_remoteLocationOccupiedByOtherPlayer).Should().BeFalse();
            Sut.CanFortify(_remoteLocationOccupiedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_fortify_to_bordering_territory_occupied_by_player()
        {
            Sut.CanFortify(_borderingLocationOccupiedByPlayer).Should().BeTrue();
        }

        [Fact]
        public void Fortifies_three_army_to_bordering_territory()
        {
            _selectedTerritory.Armies = 4;
            _borderingTerritoryOccupiedByPlayer.Armies = 10;

            Sut.Fortify(_borderingLocationOccupiedByPlayer, 3);

            _borderingTerritoryOccupiedByPlayer.Armies.Should().Be(13);
            _selectedTerritory.Armies.Should().Be(1);
        }

        [Fact]
        public void Fortify_enters_fortified_state()
        {
            var fortifiedState = Substitute.For<IInteractionState>();
            InteractionStateFactory.CreateFortifiedState(Player, WorldMap).Returns(fortifiedState);

            Sut.Fortify(_borderingLocationOccupiedByPlayer, 1);

            StateController.CurrentState.Should().Be(fortifiedState);
        }
    }
}