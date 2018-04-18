using System.Collections.Generic;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using Xunit;

namespace Tests.GameEngine.Play
{
    public class ArmiesLostCalculatorTests
    {
        private readonly ArmiesLostCalculator _sut;

        public ArmiesLostCalculatorTests()
        {
            _sut = new ArmiesLostCalculator();
        }

        public static IEnumerable<object[]> _attackerLossesCases
        {
            get
            {
                yield return new object[] { 1, new[] { 2, 1, 1 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 1, 1, 2 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 2, 1 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 1 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 1 }, new[] { 1 } };
                yield return new object[] { 2, new[] { 1, 1, 1 }, new[] { 1, 1 } };
                yield return new object[] { 2, new[] { 6, 6, 6 }, new[] { 6, 6 } };
                yield return new object[] { 1, new[] { 6, 6, 6 }, new[] { 5, 6 } };
            }
        }

        [Theory]
        [MemberData(nameof(_attackerLossesCases))]
        public void CalculateAttackerLosses(int expectedCasualties, ICollection<int> attack, ICollection<int> defence)
        {
            _sut.CalculateAttackerLosses(attack, defence).Should().Be(expectedCasualties);
        }

        public static IEnumerable<object[]> _defenderLossesCases
        {
            get
            {
                yield return new object[] { 1, new[] { 2, 1, 1 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 1, 1, 2 }, new[] { 1, 1 } };
                yield return new object[] { 1, new[] { 2, 1 }, new[] { 1, 1 } };
                yield return new object[] { 0, new[] { 1 }, new[] { 1, 1 } };
                yield return new object[] { 0, new[] { 1 }, new[] { 1 } };
                yield return new object[] { 0, new[] { 1, 1, 1 }, new[] { 1, 1 } };
                yield return new object[] { 0, new[] { 6, 6, 6 }, new[] { 6, 6 } };
                yield return new object[] { 1, new[] { 6, 6, 6 }, new[] { 5, 6 } };
            }
        }

        [Theory]
        [MemberData(nameof(_defenderLossesCases))]
        public void CalculateDefenderLosses(int expectedLosses, ICollection<int> attack, ICollection<int> defence)
        {
            _sut.CalculateDefenderLosses(attack, defence).Should().Be(expectedLosses);
        }
    }
}