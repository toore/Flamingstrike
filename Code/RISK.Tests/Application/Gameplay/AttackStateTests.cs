using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class AttackStateTests
    {
        private readonly IInteractionState _sut;
        private readonly IPlayer _player;
        private readonly IBattleCalculator _battleCalculator;
        private readonly StateController _stateController;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMap _worldMap;

        private readonly ITerritory _selectedTerritory;
        private readonly ILocation _selectedLocation;
        private readonly ILocation _borderingLocationOccupiedByOtherPlayer;
        private readonly Location _remoteLocationOccupiedByOtherPlayer;
        private readonly Location _borderingLocationOccupiedByPlayer;
        private readonly Territory _borderingTerritoryOccupiedByOtherPlayer;
        private readonly Location _remoteLocationOccupiedByPlayer;
        private readonly Territory _borderingTerritoryOccupiedByPlayer;

        public AttackStateTests()
        {
            _stateController = new StateController();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _player = Substitute.For<IPlayer>();
            _worldMap = Substitute.For<IWorldMap>();
            _battleCalculator = Substitute.For<IBattleCalculator>();

            _borderingLocationOccupiedByPlayer = Make.Location.Build();
            _borderingTerritoryOccupiedByPlayer = Make.Territory
                .Location(_borderingLocationOccupiedByPlayer)
                .Occupant(_player)
                .Build();
            _worldMap.GetTerritory(_borderingLocationOccupiedByPlayer).Returns(_borderingTerritoryOccupiedByPlayer);

            _remoteLocationOccupiedByPlayer = Make.Location.Build();
            var remoteTerritoryOccupiedByPlayer = Make.Territory
                .Location(_remoteLocationOccupiedByPlayer)
                .Occupant(_player)
                .Build();
            _worldMap.GetTerritory(_remoteLocationOccupiedByPlayer).Returns(remoteTerritoryOccupiedByPlayer);

            var otherPlayer = Substitute.For<IPlayer>();
            
            _borderingLocationOccupiedByOtherPlayer = Make.Location.Build();
            _borderingTerritoryOccupiedByOtherPlayer = Make.Territory
                .Location(_borderingLocationOccupiedByOtherPlayer)
                .Occupant(otherPlayer)
                .Build();
            _worldMap.GetTerritory(_borderingLocationOccupiedByOtherPlayer).Returns(_borderingTerritoryOccupiedByOtherPlayer);

            _remoteLocationOccupiedByOtherPlayer = Make.Location.Build();
            var remoteTerritoryOccupiedByOtherPlayer = Make.Territory
                .Location(_remoteLocationOccupiedByOtherPlayer)
                .Occupant(otherPlayer)
                .Build();
            _worldMap.GetTerritory(_remoteLocationOccupiedByOtherPlayer).Returns(remoteTerritoryOccupiedByOtherPlayer);

            _selectedLocation = Make.Location
                .WithBorder(_borderingLocationOccupiedByOtherPlayer)
                .WithBorder(_borderingLocationOccupiedByPlayer)
                .Build();
            _selectedTerritory = Make.Territory
                .Location(_selectedLocation)
                .Occupant(_player)
                .Build();
            
            _sut = new AttackState(_stateController, _interactionStateFactory, _battleCalculator, _player, _worldMap, _selectedTerritory);
        }

        [Fact]
        public void Can_click_on_location_already_selected()
        {
            _sut.CanClick(_selectedLocation).Should().BeTrue();
        }

        [Fact]
        public void Can_not_select_not_selected_location()
        {
            _sut.CanClick(Substitute.For<ILocation>()).Should().BeFalse();
            _sut.CanClick(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Selecting_selected_location_enters_select_state()
        {
            var selectState = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateSelectState(_stateController, _player, _worldMap).Returns(selectState);

            _sut.OnClick(_selectedLocation);

            _stateController.CurrentState.Should().Be(selectState);
        }

        [Fact]
        public void Can_not_click_bordering_location_when_having_one_army()
        {
            _selectedTerritory.Armies = 1;

            _sut.CanClick(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_non_bordering_location()
        {
            _selectedTerritory.Armies = 2;

            _sut.CanClick(_remoteLocationOccupiedByOtherPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_not_attack_bordering_location_already_occupied()
        {
            _selectedTerritory.Armies = 2;

            _sut.CanClick(_borderingLocationOccupiedByPlayer).Should().BeFalse();
        }

        [Fact]
        public void Can_attack_bordering_location_when_having_two_armies()
        {
            _selectedTerritory.Armies = 2;

            _sut.CanClick(_borderingLocationOccupiedByOtherPlayer).Should().BeTrue();
        }

        [Fact]
        public void Attacks_when_territories_have_borders()
        {
            _selectedTerritory.Armies = 2;

            _sut.OnClick(_borderingLocationOccupiedByOtherPlayer);

            _battleCalculator.Received().Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer);
        }

        [Fact]
        public void After_successfull_attack_attack_enters_attack_state()
        {
            var expected = Substitute.For<IInteractionState>();
            _interactionStateFactory.CreateAttackState(_stateController, _player, _worldMap, _borderingTerritoryOccupiedByOtherPlayer).Returns(expected);
            _selectedTerritory.Armies = 2;
            AttackerWins();

            _sut.OnClick(_borderingLocationOccupiedByOtherPlayer);

            _stateController.CurrentState.Should().Be(expected);
        }

        private void AttackerWins()
        {
            _battleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);
        }

        [Fact]
        public void Player_will_be_awarded_a_card_when_turn_ends()
        {
            _selectedTerritory.Armies = 2;
            _battleCalculator
                .When(x => x.Attack(_selectedTerritory, _borderingTerritoryOccupiedByOtherPlayer))
                .Do(x => _borderingTerritoryOccupiedByOtherPlayer.Occupant = _selectedTerritory.Occupant);

            _sut.OnClick(_borderingLocationOccupiedByOtherPlayer);

            _stateController.PlayerShouldReceiveCardWhenTurnEnds.Should().BeTrue();
        }

        //[Fact]
        //public void Can_not_fortify()
        //{
        //    _sut.CanFortify(_selectedLocation).Should().BeFalse();
        //    _sut.CanFortify(_borderingLocationOccupiedByOtherPlayer).Should().BeFalse();
        //    _sut.CanFortify(_remoteLocationOccupiedByOtherPlayer).Should().BeFalse();
        //    _sut.CanFortify(_remoteLocationOccupiedByPlayer).Should().BeFalse();
        //}

        //[Fact]
        //public void Can_fortify_to_bordering_territory_occupied_by_player()
        //{
        //    _sut.CanFortify(_borderingLocationOccupiedByPlayer).Should().BeTrue();
        //}

        //[Fact]
        //public void Fortifies_three_army_to_bordering_territory()
        //{
        //    _selectedTerritory.Armies = 4;
        //    _borderingTerritoryOccupiedByPlayer.Armies = 10;

        //    _sut.Fortify(_borderingLocationOccupiedByPlayer, 3);

        //    _borderingTerritoryOccupiedByPlayer.Armies.Should().Be(13);
        //    _selectedTerritory.Armies.Should().Be(1);
        //}

        //[Fact]
        //public void Fortify_enters_fortified_state()
        //{
        //    var fortifiedState = Substitute.For<IInteractionState>();
        //    _interactionStateFactory.CreateFortifiedState(_player, _worldMap).Returns(fortifiedState);

        //    _sut.Fortify(_borderingLocationOccupiedByPlayer, 1);

        //    _stateController.CurrentState.Should().Be(fortifiedState);
        //}
    }
}