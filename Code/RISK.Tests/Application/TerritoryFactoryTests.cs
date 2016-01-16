﻿using FluentAssertions;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;
using IPlayer = RISK.Application.IPlayer;
using Territory = RISK.Application.Territory;

namespace RISK.Tests.Application
{
    public class TerritoryFactoryTests
    {
        [Fact]
        public void Creates_territory_used_in_game_play()
        {
            var territory1 = Substitute.For<ITerritoryGeography>();
            var territory2 = Substitute.For<ITerritoryGeography>();
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();

            var sut = new TerritoryFactory();
            var actual = sut.Create(new[]
            {
                new Territory(territory1, player1, 1),
                new Territory(territory2, player2, 2)
            });

            actual.ShouldBeEquivalentTo(new[]
            {
                new RISK.Application.Play.Territory(territory1, player1, 1),
                new RISK.Application.Play.Territory(territory2, player2, 2)
            });
        }
    }
}