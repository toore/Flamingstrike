using System;
using System.Collections.Generic;
using System.Reflection;
using FluentAssertions;
using NSubstitute;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.DiceAndCalculation;
using Xunit;
using Xunit.Extensions;


namespace RISK.Tests.Application.Battle
{
   public class AttackCases : DataAttribute
   {
       public static readonly IPlayer Attacker = Substitute.For<IPlayer>();
       public static readonly IPlayer Defender = Substitute.For<IPlayer>();

       public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
       {
           yield return new object[] { 2, 1, 0, 1, Attacker, 1, 1 };
           yield return new object[] { 2, 2, 0, 1, Defender, 2, 1 };
           yield return new object[] { 2, 2, 1, 0, Defender, 1, 2 };
       }
   }

    public class BattleCalculatorTests
    {
        private readonly BattleCalculator _battleCalculator;
        private readonly IDices _dices;
        
        public BattleCalculatorTests()
        {
            _dices = Substitute.For<IDices>();
            _battleCalculator = new BattleCalculator(_dices);
        }

        [Theory]
        [AttackCases]
        public void Attack(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties,
            IPlayer expectedOwnerAfterAttack, int expectedArmiesInAttackingTerritoryAfter, int expectedArmiesInDefendingTerritoryAfter)
        {
            StubDices(attackingArmies - 1, defendingArmies, attackerCasualties, defenderCasualties);

            var attackerTerritory = Make.Territory.Armies(attackingArmies).Build();
            attackerTerritory.Occupant = AttackCases.Attacker;
            var defenderTerritory = Make.Territory.Armies(defendingArmies).Build();
            defenderTerritory.Occupant = AttackCases.Defender;

            _battleCalculator.Attack(attackerTerritory, defenderTerritory);

            attackerTerritory.Occupant.Should().Be(AttackCases.Attacker);
            attackerTerritory.Armies.Should().Be(expectedArmiesInAttackingTerritoryAfter);
            defenderTerritory.Occupant.Should().Be(expectedOwnerAfterAttack);
            defenderTerritory.Armies.Should().Be(expectedArmiesInDefendingTerritoryAfter);
        }

        private void StubDices(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties)
        {
            var diceResult = Substitute.For<IDicesResult>();
            diceResult.AttackerCasualties.Returns(attackerCasualties);
            diceResult.DefenderCasualties.Returns(defenderCasualties);
            _dices.Roll(attackingArmies, defendingArmies).Returns(diceResult);
        }
    }
}