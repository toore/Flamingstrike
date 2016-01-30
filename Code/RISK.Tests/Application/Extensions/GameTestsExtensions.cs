using System;
using FluentAssertions;
using RISK.Application;
using RISK.Application.Play;

namespace RISK.Tests.Application.Extensions
{
    public static class GameTestsExtensions
    {
        public static void AssertCanNotAttack(this IGame sut, ITerritory attackingTerritory, ITerritory attackedTerritory)
        {
            Action act = () => sut.Attack(attackingTerritory, attackedTerritory);

            sut.CanAttack(attackingTerritory, attackedTerritory).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotMoveArmiesIntoCapturedTerritory(this IGame sut, int numberOfArmies)
        {
            Action act = () => sut.MoveArmiesIntoOccupiedTerritory(numberOfArmies);

            sut.CanMoveArmiesIntoOccupiedTerritory().Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotFortify(this IGame sut, ITerritory attackingTerritory, ITerritory attackedTerritory)
        {
            Action act = () => sut.Fortify(attackingTerritory, attackedTerritory);

            sut.CanFortify(attackingTerritory, attackedTerritory).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}