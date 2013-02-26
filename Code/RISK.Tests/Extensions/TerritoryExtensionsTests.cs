using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using Rhino.Mocks;

namespace RISK.Tests.Extensions
{
    [TestFixture]
    public class TerritoryExtensionsTests
    {
        [Test]
        public void HasOwner_is_true()
        {
            new Territory(null) { Owner = MockRepository.GenerateStub<IPlayer>() }.HasOwner().Should().BeTrue();
        }

        [Test]
        public void HasOwner_is_false()
        {
            new Territory(null).HasOwner().Should().BeFalse();
        }
    }
}