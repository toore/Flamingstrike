using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class TerritoryTests
    {
        [Test]
        public void Is_assigned_to_player()
        {
            var territory = new Territory(null);
            territory.AssignedPlayer = Substitute.For<IPlayer>();

            territory.IsPlayerAssigned().Should().BeTrue();
        }

        [Test]
        public void Is_not_assigned_to_player()
        {
            new Territory(null).IsPlayerAssigned().Should().BeFalse();
        }

        [Test]
        public void Get_armies_to_attack_with_should_be_0()
        {
            AssertArmiesToAttackWith(expected: 0, totalArmies: 0);
        }

        [Test]
        public void Get_armies_to_attack_with_should_be_1()
        {
            AssertArmiesToAttackWith(expected: 1, totalArmies: 2);
        }

        private static void AssertArmiesToAttackWith(int expected, int totalArmies)
        {
            new Territory(null) { Armies = totalArmies }.GetArmiesToAttackWith().Should().Be(expected);
        }
    }
}