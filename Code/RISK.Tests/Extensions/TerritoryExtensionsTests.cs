using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace RISK.Tests.Extensions
{
    [TestFixture]
    public class TerritoryExtensionsTests
    {
        [Test]
        public void Is_assigned_to_player()
        {
            new Territory(null) { AssignedToPlayer = Substitute.For<IPlayer>() }.IsAssignedToPlayer().Should().BeTrue();
        }

        [Test]
        public void Is_not_assigned_to_player()
        {
            new Territory(null).IsAssignedToPlayer().Should().BeFalse();
        }
    }
}