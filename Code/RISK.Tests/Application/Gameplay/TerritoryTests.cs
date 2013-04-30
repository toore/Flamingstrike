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
        public void Initializes_location()
        {
            var location = Substitute.For<ILocation>();
            var territory = new Territory(location);

            territory.Location.Should().Be(location);
        }
    }
}