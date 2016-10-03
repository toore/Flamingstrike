using System.Collections.Generic;
using FluentAssertions;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using Tests.RISK.GameEngine.Builders;
using Xunit;

namespace Tests.RISK.GameEngine.Play
{
    public class TerritoriesContextTests
    {
        private readonly TerritoriesContext _sut;

        public TerritoriesContextTests()
        {
            _sut = new TerritoriesContext();
        }

        [Fact]
        public void Gets_territory()
        {
            IReadOnlyList<ITerritory> territories = new[] { Make.Territory.Build(), Make.Territory.Build() };

            _sut.Set(territories);

            _sut.Territories.Should().BeEquivalentTo(territories);
        }
    }
}