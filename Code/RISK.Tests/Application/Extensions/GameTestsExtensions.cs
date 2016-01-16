using System;
using FluentAssertions;
using RISK.Application.Play;
using RISK.Application.World;

namespace RISK.Tests.Application.Extensions
{
    public static class GameTestsExtensions
    {
        public static void AssertCanNotAttack(this IGame sut, ITerritoryGeography attackingTerritoryGeography, ITerritoryGeography attackedTerritoryGeography)
        {
            Action act = () => sut.Attack(attackingTerritoryGeography, attackedTerritoryGeography);

            sut.CanAttack(attackingTerritoryGeography, attackedTerritoryGeography).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotMoveArmiesIntoCapturedTerritory(this IGame sut, int numberOfArmies)
        {
            Action act = () => sut.MoveArmiesIntoCapturedTerritory(numberOfArmies);

            sut.CanMoveArmiesIntoCapturedTerritory().Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotFortify(this IGame sut, ITerritoryGeography attackingTerritoryGeography, ITerritoryGeography attackedTerritoryGeography)
        {
            Action act = () => sut.Fortify(attackingTerritoryGeography, attackedTerritoryGeography);

            sut.CanFortify(attackingTerritoryGeography, attackedTerritoryGeography).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}