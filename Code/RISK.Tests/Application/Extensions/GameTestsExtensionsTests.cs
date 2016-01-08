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
        private readonly ITerritoryId _territoryId;
        private readonly ITerritoryId _anotherTerritoryId;
        private const int _numberOfArmies = 1;

        public GameTestsExtensionsTests()
        {
            _territoryId = Substitute.For<ITerritoryId>();
            _anotherTerritoryId = Substitute.For<ITerritoryId>();

            _sut = Substitute.For<IGame>();
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_CanAttack_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotAttack(_territoryId, _anotherTerritoryId),
                x => x.CanAttack(_territoryId, _anotherTerritoryId),
                x => x.Attack(_territoryId, _anotherTerritoryId));
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_Attack_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotAttack(_territoryId, _anotherTerritoryId));
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
                x => x.AssertCanNotFortify(_territoryId, _anotherTerritoryId),
                x => x.CanFortify(_territoryId, _anotherTerritoryId),
                x => x.Fortify(_territoryId, _anotherTerritoryId));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_Fortify_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotFortify(_territoryId, _anotherTerritoryId));
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