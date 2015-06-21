using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Equivalency;
using RISK.Application.Play;
using RISK.Application.Setup;

namespace RISK.Tests.Application
{
    public static class FluentAssertionsExtensions
    {
        public static void ShouldAllBeEquivalentToInRisk(this IEnumerable<IGameboardSetupTerritory> subject, IEnumerable<IGameboardSetupTerritory> expectation)
        {
            subject.ShouldAllBeEquivalentTo(expectation, RiskEquivalencyAssertionOptions.Config);
        }

        public static void ShouldAllBeEquivalentToInRisk(this IEnumerable<IGameboardTerritory> subject, IEnumerable<IGameboardTerritory> expectation)
        {
            subject.ShouldAllBeEquivalentTo(expectation, RiskEquivalencyAssertionOptions.Config);
        }

        private static class RiskEquivalencyAssertionOptions
        {
            public static EquivalencyAssertionOptions<IGameboardSetupTerritory> Config(EquivalencyAssertionOptions<IGameboardSetupTerritory> config)
            {
                return config
                    .Including(opt => opt.Territory)
                    .Including(opt => opt.Player);
            }

            public static EquivalencyAssertionOptions<IGameboardTerritory> Config(EquivalencyAssertionOptions<IGameboardTerritory> config)
            {
                return config
                    .Including(opt => opt.Territory)
                    .Including(opt => opt.Player);
            }
        }
    }
}