using System;
using FluentAssertions;
using RISK.Application.Play;
using RISK.Application.World;

namespace RISK.Tests.Application.Extensions
{
    public static class GameTestsExtensions
    {
        public static void AssertCanNotAttack(this IGame sut, IRegion attackingRegion, IRegion defendingRegion)
        {
            Action act = () => sut.Attack(attackingRegion, defendingRegion);

            sut.CanAttack(attackingRegion, defendingRegion).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotSendInArmiesToOccupy(this IGame sut, int numberOfArmies)
        {
            Action act = () => sut.SendArmiesToOccupy(numberOfArmies);

            sut.CanSendArmiesToOccupy().Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }

        public static void AssertCanNotFortify(this IGame sut, IRegion sourceRegion, IRegion destinationRegion)
        {
            Action act = () => sut.Fortify(sourceRegion, destinationRegion, 1);

            sut.CanFortify(sourceRegion, destinationRegion).Should().BeFalse();
            act.ShouldThrow<InvalidOperationException>();
        }
    }
}