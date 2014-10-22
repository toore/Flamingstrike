using System;
using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TurnTests_WhenAttacking : TurnTestsBase
    {
        private readonly TurnSelectState _sut;
        private readonly IPlayer _player;
        private readonly IBattleCalculator _battleCalculator;
        private readonly ILocation _locationOwnedByPlayer;
        private readonly ITerritory _territoryOwnedByPlayer;
        private readonly ILocation _otherLocation;
        private readonly ITerritory _otherTerritory;

        public TurnTests_WhenAttacking()
        {
            _player = Substitute.For<IPlayer>();
            var worldMap = Substitute.For<IWorldMap>();
            _battleCalculator = Substitute.For<IBattleCalculator>();
            var cardFactory = Substitute.For<ICardFactory>();

            _sut = new TurnSelectState(_player, worldMap, _battleCalculator, cardFactory);

            _locationOwnedByPlayer = Substitute.For<ILocation>();
            _territoryOwnedByPlayer = StubTerritory(worldMap, _locationOwnedByPlayer, _player);

            _otherLocation = Substitute.For<ILocation>();
            var otherPlayer = Substitute.For<IPlayer>();
            _otherTerritory = StubTerritory(worldMap, _otherLocation, otherPlayer);
        }

        [Fact]
        public void Cant_attack_when_no_location_is_selected()
        {
            _sut.CanAttack(null).Should().BeFalse("no territory has been selected");
        }

        [Fact]
        public void Attack_does_not_throw_when_no_location_is_selected()
        {
            Action attack = () => _sut.Attack(null);

            attack.ShouldNotThrow();
        }

        [Fact]
        public void Cant_attack_when_territories_are_not_connected()
        {
            SelectAndAttack();

            _battleCalculator.DidNotReceiveWithAnyArgs().Attack(null, null);
        }

        [Fact]
        public void Cant_attack_when_having_only_one_army()
        {
            _locationOwnedByPlayer.IsConnectedTo(_otherLocation);
            _territoryOwnedByPlayer.Armies = 1;
            _sut.Select(_locationOwnedByPlayer);

            _sut.CanAttack(_otherLocation).Should().BeFalse("there is only one army in location");
        }

        [Fact]
        public void Can_attack_when_territories_are_connected()
        {
            _locationOwnedByPlayer.IsConnectedTo(_otherLocation);
            _territoryOwnedByPlayer.Armies = 2;

            SelectAndAttack();

            _battleCalculator.Received().Attack(_territoryOwnedByPlayer, _otherTerritory);
        }

        [Fact]
        public void When_attack_succeeds_selected_territory_is_updated_to_occupied()
        {
            _locationOwnedByPlayer.IsConnectedTo(_otherLocation);
            _territoryOwnedByPlayer.Armies = 2;
            _battleCalculator.AttackerAlwaysWins(_territoryOwnedByPlayer, _otherTerritory);

            SelectAndAttack();

            _sut.SelectedTerritory.Should().Be(_otherTerritory, "attack suceeded so army should be moved into territory");
        }

        [Fact]
        public void Player_should_not_receive_a_card_when_attack_fails_and_turn_ends()
        {
            SelectAndAttack();
            EndTurn();

            _player.DidNotReceive().AddCard(null);
        }

        [Fact]
        public void Player_should_receive_a_card_when_attack_succeeds_and_turn_ends()
        {
            _locationOwnedByPlayer.IsConnectedTo(_otherLocation);
            _territoryOwnedByPlayer.Armies = 2;
            _battleCalculator.AttackerAlwaysWins(_territoryOwnedByPlayer, _otherTerritory);

            SelectAndAttack();
            EndTurn();

            _player.Received().AddCard(null);
        }

        [Fact]
        public void Player_should_not_receive_card_when_turn_ends()
        {
            _sut.EndTurn();

            _player.DidNotReceive().AddCard(null);
        }

        private void SelectAndAttack()
        {
            _sut.Select(_locationOwnedByPlayer);
            _sut.Attack(_otherLocation);
        }

        private void EndTurn()
        {
            _sut.EndTurn();
        }
    }

    static class TurnTestsExtensions
    {
        public static void IsConnectedTo(this ILocation from, params ILocation[] to)
        {
            from.Connections.Returns(to);
        }

        public static void AttackerAlwaysWins(this IBattleCalculator battleCalculator, ITerritory from, ITerritory to)
        {
            battleCalculator
                .When(x => x.Attack(from, to))
                .Do(x => to.Occupant = from.Occupant);
        }
    }
}