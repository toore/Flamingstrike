using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Tests.Dice
{
    [TestFixture]
    public class DiceValueCalculatorTests
    {
        private DiceValueCalculator _diceValueCalculator;

        [SetUp]
        public void SetUp()
        {
            _diceValueCalculator = new DiceValueCalculator();
        }

        //[TestCase(1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One })]
        //[TestCase(1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One })]
        //[TestCase(1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One })]
        //[TestCase(1, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One })]
        //[TestCase(1, new[] { DiceValue.One }, new[] { DiceValue.One })]
        //[TestCase(2, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One})]
        //[TestCase(2, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six })]
        //[TestCase(1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six })]

        [Test]
        public void CalculateAttackerCasualties()
        {
            AssertAttackerCasualties(1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertAttackerCasualties(1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One });
            AssertAttackerCasualties(1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertAttackerCasualties(1, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertAttackerCasualties(1, new[] { DiceValue.One }, new[] { DiceValue.One });
            AssertAttackerCasualties(2, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertAttackerCasualties(2, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six });
            AssertAttackerCasualties(1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six });
        }

        [Test]
        public void CalculateDefenderCasualties()
        {
            AssertDefenderCasualties(1, new[] { DiceValue.Two, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertDefenderCasualties(1, new[] { DiceValue.One, DiceValue.One, DiceValue.Two }, new[] { DiceValue.One, DiceValue.One });
            AssertDefenderCasualties(1, new[] { DiceValue.Two, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertDefenderCasualties(0, new[] { DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertDefenderCasualties(0, new[] { DiceValue.One }, new[] { DiceValue.One });
            AssertDefenderCasualties(0, new[] { DiceValue.One, DiceValue.One, DiceValue.One }, new[] { DiceValue.One, DiceValue.One });
            AssertDefenderCasualties(0, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Six, DiceValue.Six });
            AssertDefenderCasualties(1, new[] { DiceValue.Six, DiceValue.Six, DiceValue.Six }, new[] { DiceValue.Five, DiceValue.Six });
        }

        public void AssertAttackerCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _diceValueCalculator.CalculateAttackerCasualties(attacker, defender).Should().Be(expectedCasualties);
        }

        public void AssertDefenderCasualties(int expectedCasualties, IEnumerable<DiceValue> attacker, IEnumerable<DiceValue> defender)
        {
            _diceValueCalculator.CalculateDefenderCasualties(attacker, defender).Should().Be(expectedCasualties);
        }
    }
}