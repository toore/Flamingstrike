using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;

namespace RISK.Tests.Application.Battle
{
    [TestFixture]
    public class BattleCalculatorTests
    {
        private BattleCalculator _battleCalculator;
        private IDices _dices;
        private static readonly IPlayer _attacker = Substitute.For<IPlayer>();
        private static readonly IPlayer _defender = Substitute.For<IPlayer>();

        [SetUp]
        public void SetUp()
        {
            _dices = Substitute.For<IDices>();
            _battleCalculator = new BattleCalculator(_dices);
        }

        private static readonly object[] _attackCases =
        {
            new object[] { 2, 1, 0, 1, _attacker, 1, 1 },
            new object[] { 2, 2, 0, 1, _defender, 2, 1 },
            new object[] { 2, 2, 1, 0, _defender, 1, 2 }
        };

        [TestCaseSource("_attackCases")]
        public void Attack(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties,
            IPlayer expectedOwnerAfterAttack, int expectedArmiesInAttackingTerritoryAfter, int expectedArmiesInDefendingTerritoryAfter)
        {
            StubDices(attackingArmies - 1, defendingArmies, attackerCasualties, defenderCasualties);

            var attackerTerritory = new Territory(new Location("attacker territory", new Continent()))
            {
                Armies = attackingArmies
            };
            attackerTerritory.Occupant = _attacker;
            var defenderTerritory = new Territory(new Location("defender territory", new Continent()))
            {
                Armies = defendingArmies
            };
            defenderTerritory.Occupant = _defender;

            _battleCalculator.Attack(attackerTerritory, defenderTerritory);

            attackerTerritory.Occupant.Should().Be(_attacker);
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