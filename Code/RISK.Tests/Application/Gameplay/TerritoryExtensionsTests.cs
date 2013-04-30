using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class TerritoryExtensionsTests
    {
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