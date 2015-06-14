using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GuiWpf.TerritoryModels;
using RISK.Application;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class WorldMapModelFactoryTests
    {
        private const int NumberOfTerritories = 42;

        [Fact]
        public void Initializes_all_territory_models()
        {
            var territoryModels = Create();

            territoryModels.Should().HaveCount(NumberOfTerritories);
        }

        [Fact]
        public void All_territory_models_have_unique_properties()
        {
            var territoryModels = Create().ToList();

            territoryModels.Select(x => x.Territory).Distinct().Should().HaveCount(42);
            territoryModels.Select(x => x.Name).Distinct().Should().HaveCount(42);
            territoryModels.Select(x => x.NamePosition).Distinct().Should().HaveCount(42);
            territoryModels.Select(x => x.Path).Distinct().Should().HaveCount(42);
        }

        private IEnumerable<ITerritoryModel> Create()
        {
            var worldMap = new WorldMap();
            var sut = new WorldMapModelFactory();

            return sut.Create(worldMap);
        }
    }
}