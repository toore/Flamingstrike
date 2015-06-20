using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Equivalency;
using RISK.Application;
using RISK.Application.Setup;

namespace RISK.Tests.Application
{
    public static class FluentAssertionsExtensions
    {
        public static void ShouldAllBeEquivalentToInRisk(this IEnumerable<IGameboardTerritory> subject, IEnumerable<IGameboardTerritory> expectation)
        {
            subject.ShouldAllBeEquivalentTo(expectation, RiskEquivalencyAssertionOptions.Config);
        }

        private static class RiskEquivalencyAssertionOptions
        {
            public static EquivalencyAssertionOptions<IGameboardTerritory> Config(EquivalencyAssertionOptions<IGameboardTerritory> config)
            {
                return config
                    .Including(opt => opt.Territory)
                    .Including(opt => opt.Player);
            }
        }
    }
}