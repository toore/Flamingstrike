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
    }
}