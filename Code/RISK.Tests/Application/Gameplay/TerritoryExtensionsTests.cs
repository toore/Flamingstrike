using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class TerritoryExtensionsTests
    {
        [Test]
        public void Is_assigned_to_player()
        {
            var territory = new Territory(null) { Occupant = Substitute.For<IPlayer>() };

            territory.IsOccupied().Should().BeTrue();
        }

        [Test]
        public void Is_not_assigned_to_player()
        {
            new Territory(null).IsOccupied().Should().BeFalse();
        }

        [Test]
        [TestCase(0, 1, TestName = "One army gives one zero armies to attack with")]
        [TestCase(1, 2, TestName = "Two armies gives one army to attack with")]
        [TestCase(9, 10, TestName = "Ten armies gives nine armies to attack with")]
        public void Get_armies_to_attack_with_should_be_1(int expected, int armies)
        {
            new Territory(null) { Armies = armies }.GetArmiesToAttackWith().Should().Be(expected);
        }

        [Test]
        [TestCase(false, 1)]
        [TestCase(true, 2)]
        [TestCase(true, 10)]
        public void Has_armies_to_attack_with(bool expected, int armies)
        {
            new Territory(null) { Armies = armies }.HasArmiesToAttackWith().Should().Be(expected);
        }
    }
}