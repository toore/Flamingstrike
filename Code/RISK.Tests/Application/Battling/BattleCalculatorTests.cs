using System.Collections.Generic;
using System.Reflection;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play.Battling;
using Xunit;
using Xunit.Sdk;

namespace RISK.Tests.Application.Battling
{
   public class AttackCases : DataAttribute
   {
       public static readonly IPlayer Attacker = Substitute.For<IPlayer>();
       public static readonly IPlayer Defender = Substitute.For<IPlayer>();

       public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
       {
           yield return new object[] { 2, 1, 0, 1, Attacker, 1, 1 };
           yield return new object[] { 2, 2, 0, 1, Defender, 2, 1 };
           yield return new object[] { 2, 2, 1, 0, Defender, 1, 2 };
       }
   }

    public class BattleCalculatorTests
    {
        private readonly BattleCalculator _sut;
        
        public BattleCalculatorTests()
        {
            _sut = new BattleCalculator();
        }

        [Theory]
        [AttackCases]
        public void Attack(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties,
            IPlayer expectedOwnerAfterAttack, int expectedArmiesInAttackingTerritoryAfter, int expectedArmiesInDefendingTerritoryAfter)
        {
            StubDices(attackingArmies - 1, defendingArmies, attackerCasualties, defenderCasualties);

            //var attackerTerritory = Make.Territory.Armies(attackingArmies).Build();
            //attackerTerritory.Occupant = AttackCases.Attacker;
            //var defenderTerritory = Make.Territory.Armies(defendingArmies).Build();
            //defenderTerritory.Occupant = AttackCases.Defender;

            //_sut.Attack(attackerTerritory, defenderTerritory);

            //attackerTerritory.Occupant.Should().Be(AttackCases.Attacker);
            //attackerTerritory.Armies.Should().Be(expectedArmiesInAttackingTerritoryAfter);
            //defenderTerritory.Occupant.Should().Be(expectedOwnerAfterAttack);
            //defenderTerritory.Armies.Should().Be(expectedArmiesInDefendingTerritoryAfter);
        }

        private void StubDices(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties)
        {
            //var diceResult = Substitute.For<IDicesResult>();
            //diceResult.AttackerCasualties.Returns(attackerCasualties);
            //diceResult.DefenderCasualties.Returns(defenderCasualties);
            //_diceRoller.Roll(attackingArmies, defendingArmies).Returns(diceResult);
        }
    }
}