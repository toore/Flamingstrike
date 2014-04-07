﻿using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.GamePlaying.Setup;

namespace RISK.Tests.Application.Gameplay.Setup
{
    [TestFixture]
    public class SetupArmiesExtensionsTests
    {
        [Test]
        public void Has_armies_left()
        {
            new List<PlayerDuringGameSetup>
                {
                    new PlayerDuringGameSetup(null, 1),
                    new PlayerDuringGameSetup(null, 0)
                }
                .AnyArmiesLeft().Should().BeTrue("one player has one army left");
        }

        [Test]
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