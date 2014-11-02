using System.Collections.Generic;
using FluentAssertions;
using RISK.Application.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Gameplay.Setup
{
    public class SetupArmiesExtensionsTests
    {
        [Fact]
        public void Has_armies_left()
        {
            new List<PlayerDuringGameSetup>
            {
                new PlayerDuringGameSetup(null, 1),
                new PlayerDuringGameSetup(null, 0)
            }
            .AnyArmiesLeft().Should().BeTrue("one player has one army left");
        }

        [Fact]
        public void Do_not_have_armies_left()
        {
            new List<PlayerDuringGameSetup>
            {
                new PlayerDuringGameSetup(null, 0),
                new PlayerDuringGameSetup(null, 0)
            }
            .AnyArmiesLeft().Should().BeFalse("no player has armies left");
        }
    }
}