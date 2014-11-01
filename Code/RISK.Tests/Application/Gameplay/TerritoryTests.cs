using FluentAssertions;
using NSubstitute;
using RISK.Domain.Entities;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class TerritoryTests
    {
        [Fact]
        public void Initializes_location()
        {
            var location = Substitute.For<ILocation>();
            var territory = new Territory(location);

            territory.Location.Should().Be(location);
        }
    }
}