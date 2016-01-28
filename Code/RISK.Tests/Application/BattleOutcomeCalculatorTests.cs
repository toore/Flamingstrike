using System.Collections.Generic;
using FluentAssertions;
using RISK.Application.Play.Attacking;
using Xunit;

namespace RISK.Tests.Application
{
    public class BattleOutcomeCalculatorTests
    {
        private readonly BattleOutcomeCalculator _sut;

        public BattleOutcomeCalculatorTests()
        {
            _sut = new BattleOutcomeCalculator();
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
            _sut.Battle(attack, defence).AttackerLosses
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
            _sut.Battle(attack, defence).DefenderLosses
                .Should().Be(expectedCasualties);
        }
    }
}