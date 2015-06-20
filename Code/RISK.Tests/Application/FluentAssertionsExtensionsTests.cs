using System.Collections.Generic;
using NSubstitute;
using RISK.Application;
using RISK.Application.Setup;
using RISK.Application.World;
using Xunit;
using Xunit.Sdk;

namespace RISK.Tests.Application
{
    public class FluentAssertionsExtensionsTests
    {
        private static readonly ITerritory _territory1 = Substitute.For<ITerritory>();
        private static readonly ITerritory _territory2 = Substitute.For<ITerritory>();
        private static readonly IPlayer _player1 = Substitute.For<IPlayer>();
        private static readonly IPlayer _player2 = Substitute.For<IPlayer>();

        [Fact]
        public void Game_board_territory_object_graph_is_equivalent()
        {
            var actual = new GameboardTerritory(_territory1, _player1, 1);
            var expected = new GameboardTerritory(_territory1, _player1, 1);

            new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected });
        }

        public static IEnumerable<object[]> gameboardTerritoryNotEquivalentTestData
        {
            get
            {
                yield return new object[]
                {
                    new GameboardTerritory(_territory1, _player1, 1),
                    new GameboardTerritory(_territory2, _player1, 1),
                };
                yield return new object[]
                {
                    new GameboardTerritory(_territory1, _player1, 1),
                    new GameboardTerritory(_territory1, _player2, 1),
                };
                yield return new object[]
                {
                    new GameboardTerritory(_territory1, _player1, 1),
                    new GameboardTerritory(_territory1, _player1, 2),
                };
            }
        }

        [Theory]
        [MemberData("gameboardTerritoryNotEquivalentTestData")]
        public void Game_board_territory_object_graph_is_not_equivalent(IGameboardTerritory actual, IGameboardTerritory expected)
        {
            Assert.Throws<XunitException>(() =>
                new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected }));
        }
    }
}