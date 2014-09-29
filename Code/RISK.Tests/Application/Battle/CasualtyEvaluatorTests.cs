using System.Collections.Generic;
using FluentAssertions;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using Xunit.Extensions;

namespace RISK.Tests.Application.Battle
{
    public class CasualtyEvaluatorTests
    {
        private readonly CasualtyEvaluator _casualtyEvaluator;

        public CasualtyEvaluatorTests()
        {
            _casualtyEvaluator = new CasualtyEvaluator();
        }

        public static IEnumerable<object[]> _attackerCasualtiesCases
        {
            get
            {
                yield return new object[] { 1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.One }, new[] { DiceValue.One } };
                yield return new object[] { 2, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 2, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six } };
                yield return new object[] { 1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six } };
            }
        }

        [Theory]
        [PropertyData("_attackerCasualtiesCases")]
        public void CalculateAttackerCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _casualtyEvaluator.GetAttackerCasualties(attacker, defender).Should().Be(expectedCasualties);
        }

        public static IEnumerable<object[]> _defenderCasualtiesCases
        {
            get
            {
                yield return new object[] { 1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 0, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 0, new[] { DiceValue.One }, new[] { DiceValue.One } };
                yield return new object[] { 0, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } };
                yield return new object[] { 0, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six } };
                yield return new object[] { 1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six } };
            }
        }

        [Theory]
        [PropertyData("_defenderCasualtiesCases")]
        public void CalculateDefenderCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _casualtyEvaluator.GetDefenderCasualties(attacker, defender).Should().Be(expectedCasualties);
        }
    }
}