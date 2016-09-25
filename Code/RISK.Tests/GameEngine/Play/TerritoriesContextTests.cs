﻿using System.Collections.Generic;
using FluentAssertions;
using RISK.Core;
using RISK.GameEngine.Play;
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GameEngine.Play
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