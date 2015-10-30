using System.Collections.Generic;
using NSubstitute;
using RISK.Application;
using RISK.Application.World;
using Xunit;
using Xunit.Sdk;

namespace RISK.Tests.Application
{
    public abstract class FluentAssertionsExtensionsTests
    {
        private static readonly ITerritoryId _territory1 = Substitute.For<ITerritoryId>();
        private static readonly ITerritoryId _territory2 = Substitute.For<ITerritoryId>();
        private static readonly IPlayerId _player1 = Substitute.For<IPlayerId>();
        private static readonly IPlayerId _player2 = Substitute.For<IPlayerId>();

        public class Game_board_setup_territory_object_graph_comparison : FluentAssertionsExtensionsTests
        {
            [Fact]
            public void Is_equivalent()
            {
                var actual = new Territory(_territory1, _player1, 1);
                var expected = new Territory(_territory1, _player1, 1);

                new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected });
            }

            [Theory]
            [MemberData("_notEquivalentData")]
            public void Is_not_equivalent(ITerritory actual, ITerritory expected)
            {
                Assert.Throws<XunitException>(() =>
                    new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected }));
            }

            public static IEnumerable<object[]> _notEquivalentData
            {
                get
                {
                    yield return new object[]
                    {
                        new Territory(_territory1, _player1, 1),
                        new Territory(_territory2, _player1, 1),
                    };
                    yield return new object[]
                    {
                        new Territory(_territory1, _player1, 1),
                        new Territory(_territory1, _player2, 1),
                    };
                    yield return new object[]
                    {
                        new Territory(_territory1, _player1, 1),
                        new Territory(_territory1, _player1, 2),
                    };
                }
            }
        }

        public class Game_board_territory_object_graph_comparison : FluentAssertionsExtensionsTests
        {
            [Fact]
            public void Is_equivalent()
            {
                var actual = new RISK.Application.Play.Territory(_territory1, _player1, 1);
                var expected = new RISK.Application.Play.Territory(_territory1, _player1, 1);

                new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected });
            }

            [Theory]
            [MemberData("_notEquivalentData")]
            public void Is_not_equivalent(RISK.Application.Play.ITerritory actual, RISK.Application.Play.ITerritory expected)
            {
                Assert.Throws<XunitException>(() =>
                    new[] { actual }.ShouldAllBeEquivalentToInRisk(new[] { expected }));
            }

            public static IEnumerable<object[]> _notEquivalentData
            {
                get
                {
                    yield return new object[]
                    {
                        new RISK.Application.Play.Territory(_territory1, _player1, 1),
                        new RISK.Application.Play.Territory(_territory2, _player1, 1),
                    };
                    yield return new object[]
                    {
                        new RISK.Application.Play.Territory(_territory1, _player1, 1),
                        new RISK.Application.Play.Territory(_territory1, _player2, 1),
                    };
                    yield return new object[]
                    {
                        new RISK.Application.Play.Territory(_territory1, _player1, 1),
                        new RISK.Application.Play.Territory(_territory1, _player1, 2),
                    };
                }
            }
        }
    }
}