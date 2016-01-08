using System;
using FluentAssertions;
using RISK.Application.Play;
using RISK.Application.World;

namespace RISK.Tests.Application.Extensions
{
    public static class GameTestsExtensions
    {
        public static void AssertCanNotAttack(this IGame sut, ITerritoryId attackingTerritoryId, ITerritoryId attackedTerritoryId)
        {
            Action act = () => sut.Attack(attackingTerritoryId, attackedTerritoryId);

            sut.CanAttack(attackingTerritoryId, attackedTerritoryId).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotMoveArmiesIntoCapturedTerritory(this IGame sut, int numberOfArmies)
        {
            Action act = () => sut.MoveArmiesIntoCapturedTerritory(numberOfArmies);

            sut.CanMoveArmiesIntoCapturedTerritory().Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotFortify(this IGame sut, ITerritoryId attackingTerritoryId, ITerritoryId attackedTerritoryId)
        {
            Action act = () => sut.Fortify(attackingTerritoryId, attackedTerritoryId);

            sut.CanFortify(attackingTerritoryId, attackedTerritoryId).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}