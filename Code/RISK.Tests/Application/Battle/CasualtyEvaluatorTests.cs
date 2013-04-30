﻿using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Tests.Application.Battle
{
    [TestFixture]
    public class CasualtyEvaluatorTests
    {
        private CasualtyEvaluator _casualtyEvaluator;

        private static readonly object[] _attackerCasualtiesCases =
            {
                new object[] { 1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.One }, new[] { DiceValue.One } },
                new object[] { 2, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 2, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six } },
                new object[] { 1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six } }
            };

        private static readonly object[] _defenderCasualtiesCases =
            {
                new object[] { 1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 0, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 0, new[] { DiceValue.One }, new[] { DiceValue.One } },
                new object[] { 0, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One } },
                new object[] { 0, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six } },
                new object[] { 1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six } }
            };

        [SetUp]
        public void SetUp()
        {
            _casualtyEvaluator = new CasualtyEvaluator();
        }

        [TestCaseSource("_attackerCasualtiesCases")]
        public void CalculateAttackerCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _casualtyEvaluator.GetAttackerCasualties(attacker, defender).Should().Be(expectedCasualties);
        }

        [TestCaseSource("_defenderCasualtiesCases")]
        public void CalculateDefenderCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _casualtyEvaluator.GetDefenderCasualties(attacker, defender).Should().Be(expectedCasualties);
        }
    }
}