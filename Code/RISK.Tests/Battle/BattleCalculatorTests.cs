using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using Rhino.Mocks;

namespace RISK.Tests.Battle
{
    [TestFixture]
    public class BattleCalculatorTests
    {
        private BattleCalculator _battleCalculator;
        private IDices _dices;
        private static readonly IPlayer _attacker = MockRepository.GenerateStub<IPlayer>();
        private static readonly IPlayer _defender = MockRepository.GenerateStub<IPlayer>();

        [SetUp]
        public void SetUp()
        {
            _dices = MockRepository.GenerateStub<IDices>();
            _battleCalculator = new BattleCalculator(_dices);
        }

        private static readonly object[] _attackCases =
            {
                new object[] { 2, 1, 0, 1, _attacker, 1, 1 },
                new object[] { 2, 2, 0, 1, _defender, 2, 1 },
                new object[] { 2, 2, 1, 0, _defender, 1, 2 },
            };

        [TestCaseSource("_attackCases")]
        public void Attack(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties,
            IPlayer expectedOwnerAfterAttack, int expectedArmiesInAttackingTerritoryAfter, int expectedArmiesInDefendingTerritoryAfter)
        {
            StubDices(attackingArmies, defendingArmies, attackerCasualties, defenderCasualties);

            var attacker = new Territory(new Location("attacker territory", new Continent())) { AssignedToPlayer = _attacker, Armies = attackingArmies };
            var defender = new Territory(new Location("defender territory", new Continent())) { AssignedToPlayer = _defender, Armies = defendingArmies };

            _battleCalculator.Attack(attacker, defender);

            attacker.AssignedToPlayer.Should().Be(_attacker);
            attacker.Armies.Should().Be(expectedArmiesInAttackingTerritoryAfter);
            defender.AssignedToPlayer.Should().Be(expectedOwnerAfterAttack);
            defender.Armies.Should().Be(expectedArmiesInDefendingTerritoryAfter);
        }

        private void StubDices(int attackingArmies, int defendingArmies, int attackerCasualties, int defenderCasualties)
        {
            var diceResult = MockRepository.GenerateStub<IDicesResult>();
            diceResult.Stub(x => x.AttackerCasualties).Return(attackerCasualties);
            diceResult.Stub(x => x.DefenderCasualties).Return(defenderCasualties);
            _dices.Stub(x => x.Roll(attackingArmies, defendingArmies)).Return(diceResult);
        }
    }
}