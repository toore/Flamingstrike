using System.Collections.Generic;
using FlamingStrike.GameEngine.Play;
using FluentAssertions;
using Xunit;

namespace Tests.FlamingStrike.GameEngine.Attacking
{
    public class ArmiesLostCalculatorTests
    {
        private readonly ArmiesLostCalculator _sut;

        public ArmiesLostCalculatorTests()
        {
            _sut = new ArmiesLostCalculator();
        }

        public static IEnumerable<object[]> _attackerCasualtiesCases
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
        [MemberData(nameof(_attackerCasualtiesCases))]
        public void CalculateAttackerCasualties(int expectedCasualties, IEnumerable<int> attack, IEnumerable<int> defence)
        {
            _sut.Calculate(attack, defence).AttackingArmiesLost
                .Should().Be(expectedCasualties);
        }

        public static IEnumerable<object[]> _defenderCasualtiesCases
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
        [MemberData(nameof(_defenderCasualtiesCases))]
        public void CalculateDefenderCasualties(int expectedCasualties, IEnumerable<int> attack, IEnumerable<int> defence)
        {
            _sut.Calculate(attack, defence).DefendingArmiesLost
                .Should().Be(expectedCasualties);
        }
    }
}