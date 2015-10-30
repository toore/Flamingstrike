using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Equivalency;
using ITerritory = RISK.Application.Play.ITerritory;

namespace RISK.Tests.Application
{
    public static class FluentAssertionsExtensions
    {
        public static void ShouldAllBeEquivalentToInRisk(this IEnumerable<RISK.Application.ITerritory> subject, IEnumerable<RISK.Application.ITerritory> expectation)
        {
            subject.ShouldAllBeEquivalentTo(expectation, RiskEquivalencyAssertionOptions.Config);
        }

        public static void ShouldAllBeEquivalentToInRisk(this IEnumerable<ITerritory> subject, IEnumerable<ITerritory> expectation)
        {
            subject.ShouldAllBeEquivalentTo(expectation, RiskEquivalencyAssertionOptions.Config);
        }

        private static class RiskEquivalencyAssertionOptions
        {
            public static EquivalencyAssertionOptions<RISK.Application.ITerritory> Config(EquivalencyAssertionOptions<RISK.Application.ITerritory> config)
            {
                return config
                    .Including(opt => opt.TerritoryId)
                    .Including(opt => opt.PlayerId);
            }

            public static EquivalencyAssertionOptions<ITerritory> Config(EquivalencyAssertionOptions<ITerritory> config)
            {
                return config
                    .Including(opt => opt.TerritoryId)
                    .Including(opt => opt.PlayerId);
            }
        }
    }
}