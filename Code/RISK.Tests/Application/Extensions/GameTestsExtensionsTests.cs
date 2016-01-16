using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Extensions
{
    public class GameTestsExtensionsTests
    {
        private readonly IGame _sut;
        private readonly ITerritoryGeography _territoryGeography;
        private readonly ITerritoryGeography _anotherTerritoryGeography;
        private const int _numberOfArmies = 1;

        public GameTestsExtensionsTests()
        {
            _territoryGeography = Substitute.For<ITerritoryGeography>();
            _anotherTerritoryGeography = Substitute.For<ITerritoryGeography>();

            _sut = Substitute.For<IGame>();
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_CanAttack_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotAttack(_territoryGeography, _anotherTerritoryGeography),
                x => x.CanAttack(_territoryGeography, _anotherTerritoryGeography),
                x => x.Attack(_territoryGeography, _anotherTerritoryGeography));
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_Attack_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotAttack(_territoryGeography, _anotherTerritoryGeography));
        }

        [Fact]
        public void AssertCanNotMoveArmiesIntoCapturedTerritory_asserts_that_CanMoveArmiesIntoCapturedTerritory_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies),
                x => x.CanMoveArmiesIntoCapturedTerritory(),
                x => x.MoveArmiesIntoCapturedTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotMoveArmiesIntoCapturedTerritory_asserts_that_MoveArmiesIntoCapturedTerritory_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_CanFortify_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotFortify(_territoryGeography, _anotherTerritoryGeography),
                x => x.CanFortify(_territoryGeography, _anotherTerritoryGeography),
                x => x.Fortify(_territoryGeography, _anotherTerritoryGeography));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_Fortify_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotFortify(_territoryGeography, _anotherTerritoryGeography));
        }

        private void AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
            Action<IGame> assertAction,
            Func<IGame, bool> isEnabled,
            Action<IGame> action)
        {
            isEnabled.Invoke(_sut).Returns(true);
            _sut.When(action).Throw<InvalidOperationException>();

            AssertMethodThrowsAssertionFailedException(assertAction);
        }

        private void AssertMethodThrowsAssertionFailedException(Action<IGame> assertAction)
        {
            Action act = () => assertAction.Invoke(_sut);
            act.ShouldThrow<AssertionFailedException>();
        }
    }
}