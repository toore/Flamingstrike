using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace RISK.Tests
{
    [TestFixture]
    public class BattleEvaluaterTests
    {
        private BattleEvaluater _battleEvaluater;
        private IPlayer _player1;
        private IDices _dices;

        [SetUp]
        public void SetUp()
        {
            _dices = MockRepository.GenerateStub<IDices>();
            _battleEvaluater = new BattleEvaluater(_dices);

            _player1 = MockRepository.GenerateStub<IPlayer>();
        }

        [Test]
        public void Attacker_will_win_in_first_attack()
        {
            var diceResult = MockRepository.GenerateStub<IDicesResult>();
            _dices.Stub(x => x.Roll(2, 1)).Return(diceResult);
            var attacker = new Area(new AreaDefinition("attacker area", new Continent())) { Owner = _player1, Armies = 2};
            var defender = new Area(new AreaDefinition("defender area", new Continent()));

            _battleEvaluater.Attack(attacker, defender);

            attacker.Owner.Should().Be(_player1);
            attacker.Armies.Should().Be(1);
            defender.Owner.Should().Be(_player1);
            defender.Armies.Should().Be(1);
        }
    }
}