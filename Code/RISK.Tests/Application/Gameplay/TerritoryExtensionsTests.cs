using System;
using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.GamePlay;
using RISK.Tests.Builders;
using Xunit;
using Xunit.Extensions;

namespace RISK.Tests.Application.Gameplay
{
    
    public class TerritoryExtensionsTests
    {
        [Fact]
        public void Is_assigned_to_player()
        {
            var territory = Make.Territory.Occupant(Substitute.For<IPlayer>()).Build();

            //territory.IsOccupied().Should().BeTrue();
            throw new NotImplementedException();
        }

        [Fact]
        public void Is_not_assigned_to_player()
        {
            //Make.Territory.Build().IsOccupied().Should().BeFalse();
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 2)]
        [InlineData(9, 10)]
        public void Get_armies_to_attack_with_should_be_1(int expected, int armies)
        {
            //Make.Territory.Armies(armies).Build().GetArmiesAvailableForAttack().Should().Be(expected);
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData(false, 1)]
        [InlineData(true, 2)]
        [InlineData(true, 10)]
        public void Has_armies_to_attack_with(bool expected, int armies)
        {
            //Make.Territory.Armies(armies).Build().HasArmiesAvailableForAttack().Should().Be(expected);
            throw new NotImplementedException();
        }
    }
}